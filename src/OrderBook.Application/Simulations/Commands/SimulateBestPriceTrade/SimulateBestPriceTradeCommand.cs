using OrderBook.Application.Abstractions.Handlers;
using OrderBook.Application.Simulations.Enums;

namespace OrderBook.Application.Simulations.Commands.SimulateBestPriceTrade;

public record SimulateBestPriceTradeCommand(
    string Instrument,
    OperationType Operation, 
    decimal Quantity) 
    : ICommand<SimulateBestPriceTradeResult>;
