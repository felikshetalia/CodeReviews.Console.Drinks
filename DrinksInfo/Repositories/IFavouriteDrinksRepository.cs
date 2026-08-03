public interface IFavouriteDrinksRepository
{
    Task<IReadOnlyList<FavouriteDrink>> GetAllFavouritesAsync();
    Task<bool> AddAsync(FavouriteDrink drink);
}