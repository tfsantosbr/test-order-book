using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using OrderBook.Application.Simulations.Commands.SimulateBestPriceTrade;
using OrderBook.Application.Simulations.Enums;
using OrderBook.Application.Simulations.Repositories;
using OrderBook.Application.Trades;
using OrderBook.Application.Trades.Constants;
using OrderBook.Application.Trades.Repositories;

namespace OrderBook.Application.Tests.Simulations.Commands;

public class SimulateBestPriceTradeCommandHandlerTests
{
    private readonly ITradeRepository _tradeRepository;
    private readonly ISimulationRepository _simulationRepository;
    private readonly IValidator<SimulateBestPriceTradeCommand> _validator;
    private readonly SimulateBestPriceTradeCommandHandler _handler;

    public SimulateBestPriceTradeCommandHandlerTests()
    {
        _tradeRepository = Substitute.For<ITradeRepository>();
        _simulationRepository = Substitute.For<ISimulationRepository>();
        _validator = Substitute.For<IValidator<SimulateBestPriceTradeCommand>>();
        _handler = new SimulateBestPriceTradeCommandHandler(_tradeRepository, _simulationRepository, _validator);
    }

    [Fact]
    public async Task ShouldReturnError_WhenValidationFails()
    {
        // Arrange
        var command = new SimulateBestPriceTradeCommand("Instrument", OperationType.Buy, 10);
        var validationResult = new ValidationResult([new ValidationFailure("Property", "Error")]);
        _validator.Validate(command).Returns(validationResult);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(result.Notifications, n => n.Message == "Error");
    }

    [Fact]
    public async Task ShouldReturnError_WhenNoTradesFound()
    {
        // Arrange
        var command = new SimulateBestPriceTradeCommand("Instrument", OperationType.Buy, 10);
        _validator.Validate(command).Returns(new ValidationResult());
        _tradeRepository.GetTradesByInstrumentForSellAsync(command.Instrument, default).Returns([]);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(TradeErrors.TradesNotFound(), result.Notifications);
    }

    [Fact]
    public async Task ShouldReturnError_WhenNotEnoughTradesAvailable()
    {
        // Arrange
        var command = new SimulateBestPriceTradeCommand("Instrument", OperationType.Buy, 20);
        _validator.Validate(command).Returns(new ValidationResult());
        var trades = new List<Trade>
        {
            new(1, "channel", "event", 123, 10, "10", 100, "100", 1, 123, 456,675)
        };
        _tradeRepository.GetTradesByInstrumentForSellAsync(command.Instrument, default).Returns(trades);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(TradeErrors.NotEnoughTradesAvailable(), result.Notifications);
    }

    [Fact]
    public async Task ShouldSimulateBuy_WhenOperationIsBuy()
    {
        // Arrange
        var command = new SimulateBestPriceTradeCommand("Instrument", OperationType.Buy, 10);
        _validator.Validate(command).Returns(new ValidationResult());
        var trades = new List<Trade>
        {
            new(1, "channel", "event", 123, 10, "10", 100, "100", 1, 123, 456,476)
        };
        _tradeRepository.GetTradesByInstrumentForSellAsync(command.Instrument, default).Returns(trades);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(command.Quantity, result.Data!.RequestedQuantity);
    }

    [Fact]
    public async Task ShouldSimulateSell_WhenOperationIsSell()
    {
        // Arrange
        var command = new SimulateBestPriceTradeCommand("Instrument", OperationType.Sell, 10);
        _validator.Validate(command).Returns(new ValidationResult());
        var trades = new List<Trade>
        {
            new(1, "channel", "event", 123, 10, "10", 100, "100", 1, 123, 456,987)
        };
        _tradeRepository.GetTradesByInstrumentForBuyAsync(command.Instrument, default).Returns(trades);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(command.Quantity, result.Data!.RequestedQuantity);
    }
}
