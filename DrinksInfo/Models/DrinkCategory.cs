using System.Text.Json.Serialization;

public class DrinkCategory
{
    [JsonPropertyName("strCategory")]
    public required string CategoryName { get; set; }
}