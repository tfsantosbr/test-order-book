using System.Text.Json.Serialization;

namespace OrderBook.Worker.BitstampClient;

public record BitstampTradeResponse
{
    [JsonPropertyName("data")]
    public BitstampTradeResponseData Data { get; set; } = default!;

    [JsonPropertyName("channel")]
    public string Channel { get; set; } = string.Empty;

    [JsonPropertyName("event")]
    public string Event { get; set; } = string.Empty;
}

public record BitstampTradeResponseData
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; } = string.Empty;

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("amount_str")]
    public string AmountStr { get; set; } = string.Empty;

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("price_str")]
    public string PriceStr { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public int Type { get; set; }

    [JsonPropertyName("microtimestamp")]
    public string Microtimestamp { get; set; } = string.Empty;

    [JsonPropertyName("buy_order_id")]
    public long BuyOrderId { get; set; }

    [JsonPropertyName("sell_order_id")]
    public long SellOrderId { get; set; }
}

