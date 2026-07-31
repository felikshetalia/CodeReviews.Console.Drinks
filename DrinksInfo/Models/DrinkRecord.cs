using System.Text.Json.Serialization;

public class DrinkRecord
{
    [JsonPropertyName("idDrink")]
    public required int Id { get; set; }
    [JsonPropertyName("strDrink")]
    public required string Name { get; set; }
    [JsonPropertyName("strDrinkThumb")]
    public string? ImageURL { get; set; }
}