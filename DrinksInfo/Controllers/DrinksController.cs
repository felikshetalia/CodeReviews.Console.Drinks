using System.Text.Json;

namespace CodeReviews.Console.Drinks;

public sealed class DrinksController
{
    private readonly IDrinksView _drinksView;
    private readonly IDrinksService _drinksService;

    public DrinksController(IDrinksView view, IDrinksService service)
    {
        _drinksView = view;
        _drinksService = service;
    }

    public async Task ShowCategories()
    {
        try
        {
            IReadOnlyCollection<DrinkCategory> categories = await _drinksService.GetDrinkCategoriesAsync();
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
        IReadOnlyCollection<DrinkRecord> drinksList = await _drinksService.GetDrinksListAsync(category);
        _drinksView.DisplayDrinks(drinksList);
        string id = _drinksView.GetDrinkId();
        await ShowDrinkDetails(id);
    }

    public async Task ShowDrinkDetails(string drinkId)
    {
        DrinkDetails? details = await _drinksService.GetDrinkDetailsAsync(drinkId);
        if (details == null)
        {
            _drinksView.DisplayError($"No drink was found with ID '{drinkId}'.");
            return;
        }

        _drinksView.DisplayDrinkDetails(details);
        _drinksView.WaitForInput();
    }
}