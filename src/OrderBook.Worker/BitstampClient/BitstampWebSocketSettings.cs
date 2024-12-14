namespace OrderBook.Worker.BitstampClient;

public record BitstampWebSocketSettings
{
    public string Url { get; init; } = string.Empty;
    public List<string> Instruments { get; init; } = default!;
}
