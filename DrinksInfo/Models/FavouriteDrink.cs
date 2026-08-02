public sealed class FavouriteDrink
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Category { get; init; }
    public DateTimeOffset AddedAtUtc { get; init; } = DateTimeOffset.UtcNow;
}