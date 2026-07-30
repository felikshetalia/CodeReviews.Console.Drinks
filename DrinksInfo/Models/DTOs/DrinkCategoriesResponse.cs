using System.Text.Json.Serialization;

public class DrinkCategoriesResponse
{
    [JsonPropertyName("drinks")]
    public List<DrinkCategory>? Drinks { get; set; }
}