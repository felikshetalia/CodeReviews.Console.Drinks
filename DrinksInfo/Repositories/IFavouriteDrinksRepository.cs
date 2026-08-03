public interface IFavouriteDrinksRepository
{
    Task<IReadOnlyList<FavouriteDrink>> GetAllFavouritesAsync();
    Task<bool> AddAsync(FavouriteDrink drink);
    Task<bool> ContainsAsync(int drinkId);
    Task<bool> RemoveAsync(int drinkId);
}