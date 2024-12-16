using System.Text.Json.Serialization;

namespace OrderBook.Application.Trades;

public class Trade
{
    public Trade(
        long id, string channel, string @event, long timestamp, decimal amount, string amountStr, decimal price,
        string priceStr, int type, long microtimestamp, long buyOrderId, long sellOrderId)
    {
        Id = id;
        Channel = channel;
        Event = @event;
        Timestamp = timestamp;
        Amount = amount;
        AmountStr = amountStr;
        Price = price;
        PriceStr = priceStr;
        Type = type;
        Microtimestamp = microtimestamp;
        BuyOrderId = buyOrderId;
        SellOrderId = sellOrderId;
    }

    private Trade()
    {
    }

    public long Id { get; private set; }

    public string Channel { get; private set; } = string.Empty;

    public string Event { get; private set; } = string.Empty;

    public long Timestamp { get; private set; }

    public decimal Amount { get; private set; }

    public string AmountStr { get; private set; } = string.Empty;

    public decimal Price { get; private set; }

    public string PriceStr { get; private set; } = string.Empty;

    public int Type { get; private set; }

    public long Microtimestamp { get; private set; }

    public long BuyOrderId { get; private set; }

    public long SellOrderId { get; private set; }
}
