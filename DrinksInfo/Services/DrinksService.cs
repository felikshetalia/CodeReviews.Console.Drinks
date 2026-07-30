
using System.Net.Http.Json;

public sealed class DrinksService : IDrinksService
{
    private readonly HttpClient _httpClient;
    public DrinksService(HttpClient cli) => _httpClient = cli;
    public async Task<IReadOnlyCollection<DrinkCategory>> GetDrinkCategoriesAsync()
    {
        const string postfix = "list.php?c=list";
        DrinkCategoriesResponse? response = await _httpClient.GetFromJsonAsync<DrinkCategoriesResponse>(postfix);
        return response?.Drinks?
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
        throw new NotImplementedException();
    }
}