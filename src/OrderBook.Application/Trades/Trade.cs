using System.Text.Json.Serialization;

namespace OrderBook.Application.Trades;

public class Trade(
    long id, string channel, string @event, string timestamp, decimal amount, string amountStr, decimal price,
    string priceStr, int type, string microtimestamp, long buyOrderId, long sellOrderId)
{
    public long Id { get; set; } = id;

    public string Channel { get; set; } = channel;

    public string Event { get; set; } = @event;

    public string Timestamp { get; set; } = timestamp;

    public decimal Amount { get; set; } = amount;

    public string AmountStr { get; set; } = amountStr;

    public decimal Price { get; set; } = price;

    public string PriceStr { get; set; } = priceStr;

    public int Type { get; set; } = type;

    public string Microtimestamp { get; set; } = microtimestamp;

    public long BuyOrderId { get; set; } = buyOrderId;

    public long SellOrderId { get; set; } = sellOrderId;
}
