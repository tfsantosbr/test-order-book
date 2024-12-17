using FluentValidation;
using OrderBook.Application.Abstractions.Handlers;
using OrderBook.Application.Abstractions.Results;
using OrderBook.Application.Simulations.Enums;
using OrderBook.Application.Simulations.Repositories;
using OrderBook.Application.Trades;
using OrderBook.Application.Trades.Constants;
using OrderBook.Application.Trades.Repositories;

namespace OrderBook.Application.Simulations.Commands.SimulateBestPriceTrade;

public class SimulateBestPriceTradeCommandHandler(
    ITradeRepository tradeRepository, ISimulationRepository simulationRepository,
    IValidator<SimulateBestPriceTradeCommand> validator)
    : AbstractHandler<SimulateBestPriceTradeResult>, ICommandHandler<SimulateBestPriceTradeCommand, SimulateBestPriceTradeResult>
{
    // Implementations

    public async Task<Result<SimulateBestPriceTradeResult>> HandleAsync(
        SimulateBestPriceTradeCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = ValidateCommand(validator, command);

        if (validationResult.IsFailure)
            return ErrorResult(validationResult.Notifications);

        return command.Operation switch
        {
            OperationType.Buy => await SimulateBuyAsync(command, cancellationToken),
            OperationType.Sell => await SimulateSellAsync(command, cancellationToken),
            _ => throw new ArgumentException("Invalid operation type.")
        };
    }


    // Private Methods

    private static Result ValidateCommand(IValidator<SimulateBestPriceTradeCommand> validator, SimulateBestPriceTradeCommand command)
    {
        var validationResult = validator.Validate(command);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(error => new Error(error.ErrorCode, error.ErrorMessage)).ToArray();

            return Result.Error(errors);
        }

        return Result.Success();
    }

    private async Task<Result<SimulateBestPriceTradeResult>> SimulateBuyAsync(SimulateBestPriceTradeCommand command, CancellationToken cancellationToken)
    {
        var trades = await tradeRepository.GetTradesByInstrumentForSellAsync(command.Instrument, cancellationToken);

        return await SimulateTradeAsync(command, trades, cancellationToken);
    }

    private async Task<Result<SimulateBestPriceTradeResult>> SimulateSellAsync(SimulateBestPriceTradeCommand command, CancellationToken cancellationToken)
    {
        var trades = await tradeRepository.GetTradesByInstrumentForBuyAsync(command.Instrument, cancellationToken);

        return await SimulateTradeAsync(command, trades, cancellationToken);
    }

    private async Task<Result<SimulateBestPriceTradeResult>> SimulateTradeAsync(SimulateBestPriceTradeCommand command, IEnumerable<Trade> trades, CancellationToken cancellationToken)
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

        var simulation = new Simulation(
            command.Instrument,
            command.Operation,
            command.Quantity,
            totalCost,
            tradesUsed
            );

        await simulationRepository.Add(simulation, cancellationToken);

        var simulateBestPriceTradeResult = SimulateBestPriceTradeResult.FromSimulation(simulation);

        return Result.Success(simulateBestPriceTradeResult);
    }
}