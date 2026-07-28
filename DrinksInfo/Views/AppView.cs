using Spectre.Console;

public class AppView : IAppView
{
    public void DisplayGoodbye()
        => AnsiConsole.MarkupLine("[green]Goodbye![/]");

    public MainMenuOption DisplayMainMenu()
    {
        AnsiConsole.Clear();
        return AnsiConsole.Prompt(
            new SelectionPrompt<MainMenuOption>()
                .Title("[green]Welcome to JuiceNet Cafe![/]")
                .AddChoices(Enum.GetValues<MainMenuOption>())
                .UseConverter(FormatMenuOption));
    }

    public void DisplayMessage(string message)
        => AnsiConsole.MarkupLine(Markup.Escape(message));

    public void WaitForInput()
    {
        AnsiConsole.MarkupLine("\n[grey]Press any key to continue.[/]");
        AnsiConsole.Console.Input.ReadKey(true);
    }

    private static string FormatMenuOption(MainMenuOption option)
    {
        return option switch
        {
            MainMenuOption.DisplayCategories => "Display drinks menu",
            MainMenuOption.Favorites => "Go to your favoutite drinks",
            MainMenuOption.Exit => "Exit",
            _ => option.ToString()
        };
    }

}