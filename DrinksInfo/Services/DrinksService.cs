
using System.Net.Http.Json;

public sealed class DrinksService : IDrinksService
{
    private readonly HttpClient _httpClient;
    public DrinksService(HttpClient cli) => _httpClient = cli;
    public async Task<IReadOnlyCollection<DrinkCategory>> GetDrinkCategoriesAsync()
    {
        const string postfix = "list.php?c=list";
        DrinkCategoriesResponse? response = await _httpClient.GetFromJsonAsync<DrinkCategoriesResponse>(postfix);
        return response?.Categories?
                .Where(category => !string.IsNullOrWhiteSpace(category.CategoryName))
                .Select(category => new DrinkCategory
                {
                    CategoryName = category.CategoryName!.Trim()
                }).ToList() ?? [];

    }

    public async Task<DrinkDetails?> GetDrinkDetailsAsync(string drinkId)
    {
        string postfix = $"lookup.php?i={drinkId}";
        DrinkDetailsWrapper? response = await _httpClient.GetFromJsonAsync<DrinkDetailsWrapper>(postfix);
        DrinkDetailsResponse? dto = response?.Drinks?.FirstOrDefault();

        if (!Validators.TryValidateDrinkDetailsDto(dto, out int id))
            return null;

        return new DrinkDetails
        {
            Id = id,
            Name = dto!.Name!.Trim(),
            Category = new DrinkCategory
            {
                CategoryName = dto!.Category!.Trim()
            },
            Alcoholic = string.Equals(dto.Alcoholic, "Alcoholic", StringComparison.OrdinalIgnoreCase),
            Glass = Normalize(dto.Glass),
            Instructions = Normalize(dto.Instructions),
            Ingredients = MapIngredients(dto)
        };
    }

    public async Task<IReadOnlyCollection<DrinkRecord>> GetDrinksListAsync(string category)
    {
        string filterValue = category.Trim().Replace(' ', '_');

        string encodedCategory = Uri.EscapeDataString(filterValue);
        string postfix = $"filter.php?c={encodedCategory}";

        DrinksByCategoryResponse? response = await _httpClient.GetFromJsonAsync<DrinksByCategoryResponse>(postfix);
        return response?.Drinks?
                .Where(drink => !string.IsNullOrWhiteSpace(drink.Name))
                .Select(drink => new DrinkRecord
                {
                    Id = drink.Id,
                    Name = drink.Name,
                    ImageURL = drink.ImageURL
                }).ToList() ?? [];
    }

    private static IReadOnlyList<Ingredient> MapIngredients(DrinkDetailsResponse dto)
    {
        string?[] items =
        [
            dto.Ingredient1,
            dto.Ingredient2,
            dto.Ingredient3,
            dto.Ingredient4,
            dto.Ingredient5,
            dto.Ingredient6,
            dto.Ingredient7,
            dto.Ingredient8,
            dto.Ingredient9,
            dto.Ingredient10,
            dto.Ingredient11,
            dto.Ingredient12,
            dto.Ingredient13,
            dto.Ingredient14,
            dto.Ingredient15
        ];

        string?[] measures =
        [
            dto.Measure1,
            dto.Measure2,
            dto.Measure3,
            dto.Measure4,
            dto.Measure5,
            dto.Measure6,
            dto.Measure7,
            dto.Measure8,
            dto.Measure9,
            dto.Measure10,
            dto.Measure11,
            dto.Measure12,
            dto.Measure13,
            dto.Measure14,
            dto.Measure15
        ];

        var ingredients = new List<Ingredient>(items.Length);

        for (int i = 0; i < items.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(items[i]))
            {
                continue;
            }

            ingredients.Add(new Ingredient
            {
                Item = items[i]!.Trim(),
                Measure = Normalize(measures[i]),
                Unit = null
            });
        }

        return ingredients;
    }

    private static string? Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}