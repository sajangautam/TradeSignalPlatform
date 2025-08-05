namespace TradeSignalManager.Core.Entities
{
    public class TradeSignal
{
    public int Id { get; set; }

    private string _ticker = string.Empty;
    public required string Ticker
    {
        get => _ticker;
        set => _ticker = value?.ToUpperInvariant() ?? throw new ArgumentNullException(nameof(Ticker));
    }

    private string _action = string.Empty;
    public required string Action
    {
        get => _action;
        set => _action = value?.ToUpperInvariant() switch
        {
            "BUY" or "SELL" => value.ToUpperInvariant(),
            _ => throw new ArgumentException("Action must be BUY or SELL!")
        };
    }

    public decimal Price { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }
}
}