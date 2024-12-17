using OrderBook.Application.Trades;

namespace OrderBook.Application.Simulations.Commands.SimulateBestPriceTrade;

public record SimulateBestPriceTradeResult(
    Guid Id, 
    string Instrument, 
    string Operation, 
    decimal RequestedQuantity, 
    decimal TotalCost, 
    IEnumerable<Trade> TradesUsed
    );
