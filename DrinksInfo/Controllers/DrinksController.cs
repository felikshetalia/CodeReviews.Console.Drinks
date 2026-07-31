using System.Threading.Tasks;

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
        _drinksView.DisplayCategories(
            await _drinksService.GetDrinkCategoriesAsync()
            );
        string category = _drinksView.GetCategoryName();
        await ShowDrinksByCategory(category);
    }

    public async Task ShowDrinksByCategory(string category)
    {
        IReadOnlyCollection<DrinkRecord> drinksList = await _drinksService.GetDrinksListAsync(category);
        _drinksView.DisplayDrinks(drinksList);
        _drinksView.GetDrinkId();
    }
}