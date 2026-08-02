public interface IDrinksView
{
    void DisplayCategories(IReadOnlyCollection<DrinkCategory> categories);
    string GetCategoryName();
    void DisplayDrinks(IReadOnlyCollection<DrinkRecord> drinks);
    string GetDrinkId();
    void DisplayDrinkDetails(DrinkDetails drink);
    void DisplayError(string message);
    void WaitForInput();
    void DisplayMessage(string message);
    Task<T> ShowLoadingAsync<T>(string message, Func<Task<T>> op);
    string AskAddToFavourites();
    void DisplayFavourites(List<FavouriteDrink> favs);
}