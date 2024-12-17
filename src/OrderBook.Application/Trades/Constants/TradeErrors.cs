using OrderBook.Application.Abstractions.Results;

namespace OrderBook.Application.Trades.Constants;

public static class TradeErrors
{
    public static Error NotEnoughTradesAvailable() =>
        new(nameof(NotEnoughTradesAvailable), "Not enough trades available to fulfill the requested quantity.");

    public static Error TradesNotFound() =>
        new(nameof(TradesNotFound), "No trades found for the specified instrument.");
}
