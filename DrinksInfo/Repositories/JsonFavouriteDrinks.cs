using System.Text.Json;
public sealed class JsonFavouriteDrinks : IFavouriteDrinksRepository
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    public JsonFavouriteDrinks(string path) => _filePath = path;
    public async Task<bool> AddAsync(FavouriteDrink drink)
    {
        List<FavouriteDrink> favs = (await GetAllFavouritesAsync()).ToList();

        if (favs.Any(item => item.Id == drink.Id))
            return false;

        favs.Add(drink);
        await SaveAsync(favs);
        return true;
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