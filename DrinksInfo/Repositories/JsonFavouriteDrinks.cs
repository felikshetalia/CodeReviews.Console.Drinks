using System.Text.Json;
public sealed class JsonFavouriteDrinks : IFavouriteDrinksRepository
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    public JsonFavouriteDrinks(string path) => _filePath = path;
    public async Task<bool> AddAsync(FavouriteDrink drink)
    {
        List<FavouriteDrink> favs = (await GetAllFavouritesAsync()).ToList();

        if (Validators.IsDuplicateFavourite(drink.Id, favs))
            return false;

        favs.Add(drink);
        await SaveAsync(favs);
        return true;
    }

    public async Task<bool> ContainsAsync(int drinkId)
    {
        List<FavouriteDrink> favs = (await GetAllFavouritesAsync()).ToList();
        return Validators.IsDuplicateFavourite(drinkId, favs);
    }

    public async Task<IReadOnlyList<FavouriteDrink>> GetAllFavouritesAsync()
    {
        if (!File.Exists(_filePath))
            return Array.Empty<FavouriteDrink>();

        await using (FileStream stream = File.OpenRead(_filePath))
        {
            List<FavouriteDrink>? favourites =
                await JsonSerializer.DeserializeAsync<List<FavouriteDrink>>(
                    stream,
                    _jsonOptions);

            return favourites ?? [];
        }
    }

    public async Task<bool> RemoveAsync(int drinkId)
    {
        List<FavouriteDrink> favourites = (await GetAllFavouritesAsync()).ToList();

        int removedCount = favourites.RemoveAll(fav => fav.Id == drinkId);

        if (removedCount == 0)
            return false;

        await SaveAsync(favourites);
        return true;
    }

    private async Task SaveAsync(IReadOnlyList<FavouriteDrink> favs)
    {
        string? dir = Path.GetDirectoryName(_filePath);

        if (dir != null)
            Directory.CreateDirectory(dir);

        string tempPath = $"{_filePath}.tmp";

        await using (FileStream stream = File.Create(tempPath))
        {
            await JsonSerializer.SerializeAsync(stream, favs, _jsonOptions);
        }

        File.Move(tempPath, _filePath, overwrite: true);
    }
}