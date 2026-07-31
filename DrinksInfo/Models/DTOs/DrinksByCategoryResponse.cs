using System.Text.Json.Serialization;

public class DrinksByCategoryResponse
{
    [JsonPropertyName("drinks")]
    public List<DrinkRecord>? Drinks { get; set; }
}