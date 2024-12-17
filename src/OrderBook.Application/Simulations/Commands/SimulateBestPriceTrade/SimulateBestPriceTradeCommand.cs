using OrderBook.Application.Abstractions.Handlers;

namespace OrderBook.Application.Simulations.Commands.SimulateBestPriceTrade;

public record SimulateBestPriceTradeCommand(
    string Instrument, 
    string Operation, 
    decimal Quantity) 
    : ICommand<SimulateBestPriceTradeResult>;
