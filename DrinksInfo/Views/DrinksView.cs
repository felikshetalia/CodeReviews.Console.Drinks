
using Spectre.Console;

public class DrinksView : IDrinksView
{
    public void DisplayCategories(IReadOnlyCollection<DrinkCategory> categories)
    {
        AnsiConsole.Clear();

        var table = new Table().AddColumn("Category");

        foreach (var item in categories)
            table.AddRow(Markup.Escape(item.CategoryName));

        AnsiConsole.Write(table);
    }

    public void DisplayMessage(string message) => AnsiConsole.MarkupLine(Markup.Escape(message));

    public void DisplayDrinkDetails(DrinkDetails drink)
    {
        AnsiConsole.Clear();

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumns("attribute", "info")
            .HideHeaders();

        table.AddRow("Id", Markup.Escape(drink.Id.ToString()));
        table.AddRow("Name", Markup.Escape(drink.Name));
        table.AddRow("Category", Markup.Escape(drink.Category.CategoryName));
        table.AddRow("Alcohol", Markup.Escape(drink.Alcoholic ? "Alcoholic" : "Non alcoholic"));
        AddOptionalRow(table, "Glass", drink.Glass);

        string ingredientsInfo = string.Join(
        Environment.NewLine,
        drink.Ingredients.Select(ingredient =>
            string.IsNullOrWhiteSpace(ingredient.Measure)
                ? ingredient.Item
                : $"{ingredient.Item} — {ingredient.Measure} {ingredient.Unit}"));

        AddOptionalRow(table, "Ingredients", ingredientsInfo);
        AddOptionalRow(table, "Instructions", drink.Instructions);

        AnsiConsole.Write(table);
    }

    public void DisplayDrinks(IReadOnlyCollection<DrinkRecord> drinks)
    {
        AnsiConsole.Clear();

        var table = new Table().AddColumns("Id", "Name");

        foreach (var item in drinks)
            table.AddRow(Markup.Escape(item.Id.ToString()), Markup.Escape(item.Name));

        AnsiConsole.Write(table);
    }

    public void DisplayError(string message)
    {
        AnsiConsole.MarkupLine($"[red]{Markup.Escape(message)}[/]");
        WaitForInput();
    }

    public string GetCategoryName()
    => AnsiConsole.Prompt(
            new TextPrompt<string>("Enter a drink category:")
                .Validate(value => string.IsNullOrWhiteSpace(value)
                    ? ValidationResult.Error($"[red]Please enter a category.[/]")
                    : ValidationResult.Success()));

    public string GetDrinkId()
    => AnsiConsole.Prompt(
            new TextPrompt<string>("Enter the ID of the drink:")
                .Validate(value => string.IsNullOrWhiteSpace(value)
                    ? ValidationResult.Error($"[red]Please enter an ID.[/]")
                    : ValidationResult.Success()));

    public void WaitForInput()
    {
        AnsiConsole.MarkupLine("\n[grey]Press any key to continue.[/]");
        AnsiConsole.Console.Input.ReadKey(true);
    }

    public Task<T> ShowLoadingAsync<T>(string message, Func<Task<T>> op)
        => AnsiConsole.Status().Spinner(Spinner.Known.Dots).StartAsync(message, _ => op());

    public FavouriteDrinkAction AskFavouritesOption(bool isFav)
    {
        var choices = isFav
        ? new[]
        {
            FavouriteDrinkAction.Remove,
            FavouriteDrinkAction.Back
        }
        : new[]
        {
            FavouriteDrinkAction.Add,
            FavouriteDrinkAction.Back
        };

        return AnsiConsole.Prompt(
            new SelectionPrompt<FavouriteDrinkAction>()
                .Title("What would you like to do?")
                .AddChoices(choices)
                .UseConverter(action => action switch
                {
                    FavouriteDrinkAction.Add => "Add to favourites",
                    FavouriteDrinkAction.Remove => "Remove from favourites",
                    FavouriteDrinkAction.Back => "Back",
                    _ => action.ToString()
                }));
    }

    public void DisplayFavourites(IReadOnlyList<FavouriteDrink> favs)
    {
        AnsiConsole.Clear();

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumns("Id", "Name", "Category", "Added at");

        foreach (var item in favs)
            table.AddRow(
                Markup.Escape(item.Id.ToString()),
                Markup.Escape(item.Name),
                Markup.Escape(item.Category),
                Markup.Escape(item.AddedAtUtc.ToLocalTime().ToString("yyyy-MM-dd HH:mm")));

        AnsiConsole.Write(table);
    }

    private static void AddOptionalRow(Table table, string attribute, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return;

        table.AddRow(Markup.Escape(attribute), Markup.Escape(value.Trim()));
    }
}