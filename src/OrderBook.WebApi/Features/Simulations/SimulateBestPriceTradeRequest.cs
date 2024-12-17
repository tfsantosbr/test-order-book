using OrderBook.Application.Abstractions.Handlers;
using OrderBook.Application.Simulations.Commands.SimulateBestPriceTrade;
using OrderBook.Application.Simulations.Enums;

namespace OrderBook.WebApi.Features.Simulations;

public record SimulateBestPriceTradeRequest(
    string Instrument,
    OperationType Operation,
    decimal Quantity)
    : ICommand<SimulateBestPriceTradeResult>;
