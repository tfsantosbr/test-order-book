using OrderBook.Application.Simulations.Enums;
using OrderBook.Application.Trades;

namespace OrderBook.Application.Simulations.Commands.SimulateBestPriceTrade;

public record SimulateBestPriceTradeResult(
    Guid Id,
    string Instrument,
    OperationType Operation,
    decimal RequestedQuantity,
    decimal TotalCost,
    IEnumerable<Trade> TradesUsed
    )
{
    public static SimulateBestPriceTradeResult FromSimulation(Simulation simulation)
    {
        return new SimulateBestPriceTradeResult(
            simulation.Id,
            simulation.Instrument,
            simulation.Operation,
            simulation.RequestedQuantity,
            simulation.TotalCost,
            simulation.TradesUsed
        );
    }
}
