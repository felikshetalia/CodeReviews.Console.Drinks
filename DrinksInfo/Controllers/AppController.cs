namespace CodeReviews.Console.Drinks;

public sealed class AppController
{
    private readonly IAppView _appView;
    private readonly DrinksController _drinkController;

    public AppController(IAppView view, DrinksController drinkController)
    {
        _appView = view;
        _drinkController = drinkController;
    }

    public async Task Run()
    {
        bool isRunning = true;

        while (isRunning)
        {
            MainMenuOption selectedOption = _appView.DisplayMainMenu();

            switch (selectedOption)
            {
                case MainMenuOption.DisplayCategories:
                    await _drinkController.ShowCategories();
                    break;

                case MainMenuOption.Favorites:
                    await _drinkController.DisplayFavouriteDrinks();
                    break;

                case MainMenuOption.Exit:
                    isRunning = false;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(selectedOption),
                        selectedOption,
                        "Unknown menu option.");
            }
        }
        _appView.DisplayGoodbye();
    }
}