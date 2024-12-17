using OrderBook.Application.Abstractions.Handlers;
using OrderBook.Application.Abstractions.Results;
using OrderBook.Application.Trades;
using OrderBook.Application.Trades.Constants;
using OrderBook.Application.Trades.Repositories;

namespace OrderBook.Application.Simulations.Commands.SimulateBestPriceTrade;

public class SimulateBestPriceTradeCommandHandler(ITradeRepository tradeRepository)
    : AbstractHandler<SimulateBestPriceTradeResult>, ICommandHandler<SimulateBestPriceTradeCommand, SimulateBestPriceTradeResult>
{
    // Implementations

    public async Task<Result<SimulateBestPriceTradeResult>> HandleAsync(
        SimulateBestPriceTradeCommand command, CancellationToken cancellationToken = default)
    {
        return command.Operation.ToLower() switch
        {
            "buy" => await SimulateBuyAsync(command, cancellationToken),
            "sell" => await SimulateSellAsync(command, cancellationToken),
            _ => throw new ArgumentException("Invalid operation type. Must be 'buy' or 'sell'.")
        };
    }

    // Private Methods

    private async Task<Result<SimulateBestPriceTradeResult>> SimulateBuyAsync(SimulateBestPriceTradeCommand command, CancellationToken cancellationToken)
    {
        var trades = await tradeRepository.GetTradesByInstrumentForSellAsync(command.Instrument, cancellationToken);

        return await SimulateTradeAsync(command, trades);
    }

    private async Task<Result<SimulateBestPriceTradeResult>> SimulateSellAsync(SimulateBestPriceTradeCommand command, CancellationToken cancellationToken)
    {
        var trades = await tradeRepository.GetTradesByInstrumentForBuyAsync(command.Instrument, cancellationToken);

        return await SimulateTradeAsync(command, trades);
    }

    private async Task<Result<SimulateBestPriceTradeResult>> SimulateTradeAsync(SimulateBestPriceTradeCommand command, IEnumerable<Trade> trades)
    {
        if (!trades.Any())
            return ErrorResult(TradeErrors.TradesNotFound());

        decimal totalQuantity = 0;
        decimal totalCost = 0;
        var tradesUsed = new List<Trade>();

        foreach (var trade in trades)
        {
            if (totalQuantity >= command.Quantity)
                break;

            var availableQuantity = trade.Amount;
            var remainingQuantity = command.Quantity - totalQuantity;

            if (availableQuantity <= remainingQuantity)
            {
                totalCost += availableQuantity * trade.Price;
                totalQuantity += availableQuantity;
                tradesUsed.Add(trade);
            }
            else
            {
                totalCost += remainingQuantity * trade.Price;
                totalQuantity += remainingQuantity;
                tradesUsed.Add(trade);
            }
        }

        if (totalQuantity < command.Quantity)
            return ErrorResult(TradeErrors.NotEnoughTradesAvailable());

        var simulateBestPriceTradeResult = new SimulateBestPriceTradeResult(
            Guid.NewGuid(), 
            command.Instrument, 
            command.Operation, 
            command.Quantity, 
            totalCost, 
            tradesUsed
            );

        // TODO: salvar resultado

        var result = Result.Success(simulateBestPriceTradeResult);
    }
}