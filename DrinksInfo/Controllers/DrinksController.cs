using System.Text.Json;

namespace CodeReviews.Console.Drinks;

public sealed class DrinksController
{
    private readonly IDrinksView _drinksView;
    private readonly IDrinksService _drinksService;
    private readonly IFavouriteDrinksRepository _favsRepo;

    public DrinksController(IDrinksView view, IDrinksService service, IFavouriteDrinksRepository repo)
    {
        _drinksView = view;
        _drinksService = service;
        _favsRepo = repo;
    }

    public async Task ShowCategories()
    {
        try
        {
            IReadOnlyCollection<DrinkCategory> categories =
                await _drinksView.ShowLoadingAsync("Loading categories...",
                    () => _drinksService.GetDrinkCategoriesAsync());

            if (categories.Count == 0)
            {
                _drinksView.DisplayError("No drink categories were returned.");
                return;
            }
            _drinksView.DisplayCategories(categories);
            string category = _drinksView.GetCategoryName();
            if (!Validators.TryResolveCategory(category, categories, out DrinkCategory? selectedCategory, out string? err))
            {
                _drinksView.DisplayError(err!);
                return;
            }
            await ShowDrinksByCategory(selectedCategory!.CategoryName);
        }
        catch (HttpRequestException ex) when (ex.StatusCode.HasValue)
        {
            _drinksView.DisplayError($"The drinks API returned HTTP " +
                $"{(int)ex.StatusCode!} ({ex.Message}).");
        }
        catch (HttpRequestException)
        {
            _drinksView.DisplayError(
                "The drinks API could not be reached. " +
                "Check your internet connection and try again.");
        }
        catch (OperationCanceledException)
        {
            _drinksView.DisplayError("The drinks API request timed out.");
        }
        catch (JsonException e)
        {
            _drinksView.DisplayError(
                "The drinks API returned data " +
                "in an unexpected format." +
                $"{e.Message}");
        }
    }

    public async Task ShowDrinksByCategory(string category)
    {
        IReadOnlyCollection<DrinkRecord> drinksList =
            await _drinksView.ShowLoadingAsync("Loading drinks...",
                () => _drinksService.GetDrinksListAsync(category));

        if (drinksList.Count == 0)
        {
            _drinksView.DisplayError($"No drinks were found in category '{category}'.");
            return;
        }

        _drinksView.DisplayDrinks(drinksList);
        string id = _drinksView.GetDrinkId();
        if (!Validators.TryResolveDrink(id, drinksList, out DrinkRecord? selectedDrink, out string? err))
        {
            _drinksView.DisplayError(err!);
            return;
        }
        await ShowDrinkDetails(selectedDrink!.Id.ToString());
    }

    public async Task ShowDrinkDetails(string drinkId)
    {
        DrinkDetails? details =
            await _drinksView.ShowLoadingAsync("Loading...",
                () => _drinksService.GetDrinkDetailsAsync(drinkId));

        if (details == null)
        {
            _drinksView.DisplayError($"No drink was found with ID '{drinkId}'.");
            return;
        }

        bool isFav = await _favsRepo.ContainsAsync(details.Id);

        _drinksView.DisplayDrinkDetails(details);

        FavouriteDrinkAction opt = _drinksView.AskFavouritesOption(isFav);

        switch (opt)
        {
            case FavouriteDrinkAction.Add:
                await AddDrinkToFavourites(details);
                break;

            case FavouriteDrinkAction.Remove:
                await RemoveDrinkFromFavourites(details.Id);
                break;

            case FavouriteDrinkAction.Back:
                return;

            default:
                throw new ArgumentOutOfRangeException(
                    nameof(opt),
                    opt,
                    "Unknown favourite action.");
        }
    }

    public async Task<bool> AddDrinkToFavourites(DrinkDetails drink)
    {
        bool added =
            await _favsRepo.AddAsync(new FavouriteDrink
            {
                Id = drink.Id,
                Name = drink.Name,
                Category = drink.Category.CategoryName,
                AddedAtUtc = DateTimeOffset.UtcNow
            });

        _drinksView.DisplayMessage(
            added
                ? "Drink added to favourites."
                : "This drink is already in your favourites.");

        _drinksView.WaitForInput();
        return added;
    }

    public async Task<bool> RemoveDrinkFromFavourites(int id)
    {
        bool removed = await _favsRepo.RemoveAsync(id);

        _drinksView.DisplayMessage(
            removed
                ? "Drink removed from favourites."
                : "The drink was not found in favourites.");

        _drinksView.WaitForInput();
        return removed;
    }

    public async Task DisplayFavouriteDrinks()
    {
        IReadOnlyList<FavouriteDrink> favs =
        await _favsRepo.GetAllFavouritesAsync();

        if (favs.Count == 0)
        {
            _drinksView.DisplayMessage("You have no favourite drinks.");
            _drinksView.WaitForInput();
            return;
        }

        _drinksView.DisplayFavourites(favs);
        _drinksView.WaitForInput();
    }
}