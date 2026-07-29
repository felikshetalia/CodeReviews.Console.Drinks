public class DrinkDetails
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required DrinkCategory Category { get; set; }
    public bool Alcoholic { get; set; }
    public string? Glass { get; set; }
    public string? Instructions { get; set; }
    public IReadOnlyList<Ingredient> Ingredients { get; set; } = [];
}