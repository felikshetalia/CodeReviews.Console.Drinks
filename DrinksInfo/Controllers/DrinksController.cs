namespace CodeReviews.Console.Drinks;

public sealed class DrinksController
{
    private readonly IDrinksView _drinksView;

    public DrinksController(IDrinksView view)
    {
        _drinksView = view;
    }

    public void ShowCategories()
    {
        _drinksView.DisplayCategories([]);
    }
}