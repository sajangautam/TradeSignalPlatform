using System.Text.Json.Serialization;

public class Ticker
{
    public int Id { get; set; }

    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("exchange")]
    public string Exchange { get; set; } = null!;

    [JsonPropertyName("sector")]
    public string Sector { get; set; } = null!;
}
