using System.Text.Json;

namespace CodeReviews.Console.Drinks;

public sealed class DrinksController
{
    private readonly IDrinksView _drinksView;
    private readonly IDrinksService _drinksService;

    private List<FavouriteDrink> _favs;

    public DrinksController(IDrinksView view, IDrinksService service)
    {
        _drinksView = view;
        _drinksService = service;
        _favs = new();
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
            await ShowDrinksByCategory(category);
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
        catch (JsonException)
        {
            _drinksView.DisplayError(
                "The drinks API returned data " +
                "in an unexpected format.");
        }
    }

    public async Task ShowDrinksByCategory(string category)
    {
        IReadOnlyCollection<DrinkRecord> drinksList =
            await _drinksView.ShowLoadingAsync("Loading drinks...",
                () => _drinksService.GetDrinksListAsync(category));

        _drinksView.DisplayDrinks(drinksList);
        string id = _drinksView.GetDrinkId();
        await ShowDrinkDetails(id);
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

        _drinksView.DisplayDrinkDetails(details);
        if (_drinksView.AskAddToFavourites())
            if (!AddDrinkToFavourites(details))
                return;
    }

    public bool AddDrinkToFavourites(DrinkDetails drink)
    {
        if (_favs.Any(item => item.Id == drink.Id))
        {
            _drinksView.DisplayError("Item already exists in your favourites list");
            return false;
        }
        _favs.Add(new FavouriteDrink
        {
            Id = drink.Id,
            Name = drink.Name,
            Category = drink.Category.CategoryName,
        });

        return true;
    }

    public void DisplayFavouriteDrinks()
    {
        if (_favs.Count == 0)
        {
            _drinksView.DisplayMessage("No favourites to show");
            _drinksView.WaitForInput();
            return;
        }
        _drinksView.DisplayFavourites(_favs);
        _drinksView.WaitForInput();
    }
}