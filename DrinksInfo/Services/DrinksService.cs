
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
        throw new NotImplementedException();
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
}