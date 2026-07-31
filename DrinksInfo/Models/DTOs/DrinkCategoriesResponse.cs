using System.Text.Json.Serialization;

public class DrinkCategoriesResponse
{
    [JsonPropertyName("drinks")]
    public List<DrinkCategory>? Categories { get; set; }
}