public static class Validators
{
    public static bool TryResolveCategory(
        string? input,
        IReadOnlyCollection<DrinkCategory> categories,
        out DrinkCategory? selectedCategory,
        out string? errorMessage)
    {
        selectedCategory = null;

        if (string.IsNullOrWhiteSpace(input))
        {
            errorMessage = "Please enter a category.";
            return false;
        }

        selectedCategory = categories.FirstOrDefault(category =>
            category.CategoryName.Equals(input.Trim(), StringComparison.OrdinalIgnoreCase));

        if (selectedCategory == null)
        {
            errorMessage = $"'{input.Trim()}' is not one of the available categories.";
            return false;
        }

        errorMessage = null;
        return true;
    }

    public static bool TryResolveDrink(
        string? input,
        IReadOnlyCollection<DrinkRecord> drinks,
        out DrinkRecord? selectedDrink,
        out string? errorMessage)
    {
        selectedDrink = null;

        if (string.IsNullOrWhiteSpace(input))
        {
            errorMessage = "Please enter a drink ID.";
            return false;
        }

        if (!int.TryParse(input.Trim(), out int drinkId) || drinkId <= 0)
        {
            errorMessage = "The drink ID must be a positive number.";
            return false;
        }

        selectedDrink = drinks.FirstOrDefault(drink => drink.Id == drinkId);

        if (selectedDrink is null)
        {
            errorMessage = "That ID is not present in the displayed drinks list.";
            return false;
        }

        errorMessage = null;
        return true;
    }

    public static bool TryValidateDrinkDetailsDto(DrinkDetailsResponse? dto, out int drinkId)
    {
        drinkId = 0;

        return dto != null
            && int.TryParse(dto.Id, out drinkId)
            && drinkId > 0
            && !string.IsNullOrWhiteSpace(dto.Name)
            && !string.IsNullOrWhiteSpace(dto.Category);
    }

    public static bool IsDuplicateFavourite(int drinkId, IReadOnlyCollection<FavouriteDrink> favourites)
        => favourites.Any(fav => fav.Id == drinkId);

}