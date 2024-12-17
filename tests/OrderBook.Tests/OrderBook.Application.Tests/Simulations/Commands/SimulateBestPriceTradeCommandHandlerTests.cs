using NSubstitute;
using OrderBook.Application.Simulations.Commands.SimulateBestPriceTrade;
using OrderBook.Application.Trades;
using OrderBook.Application.Trades.Repositories;
using OrderBook.Application.Simulations.Repositories;

namespace OrderBook.Application.Tests.Simulations.Commands;

public class SimulateBestPriceTradeCommandHandlerTests
{
    private readonly ITradeRepository _tradeRepository;
    private readonly ISimulationRepository _simulationRepository;
    private readonly SimulateBestPriceTradeCommandHandler _handler;

    public SimulateBestPriceTradeCommandHandlerTests()
    {
        _tradeRepository = Substitute.For<ITradeRepository>();
        _simulationRepository = Substitute.For<ISimulationRepository>();
        _handler = new SimulateBestPriceTradeCommandHandler(_tradeRepository, _simulationRepository);
    }

    
}
