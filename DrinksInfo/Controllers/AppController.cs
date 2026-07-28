namespace CodeReviews.Console.Drinks;

public sealed class AppController
{
    private readonly IAppView _appView;

    public AppController(IAppView _view)
    {
        _appView = _view;
    }

    public void Run()
    {
        bool isRunning = true;

        while (isRunning)
        {
            MainMenuOption selectedOption = _appView.DisplayMainMenu();

            switch (selectedOption)
            {
                case MainMenuOption.DisplayCategories:
                    _appView.DisplayMessage("Display categories here");
                    _appView.WaitForInput();
                    break;

                case MainMenuOption.Favorites:
                    _appView.DisplayMessage("Display favourites here");
                    _appView.WaitForInput();
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