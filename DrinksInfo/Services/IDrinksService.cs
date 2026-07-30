public interface IDrinksService
{
    Task<IReadOnlyCollection<DrinkCategory>> GetDrinkCategoriesAsync();
    Task<IReadOnlyCollection<DrinkRecord>> GetDrinksListAsync(string category);
    Task<DrinkDetails?> GetDrinkDetailsAsync(string drinkId);
}