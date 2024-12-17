using OrderBook.Application.Abstractions.Handlers;
using OrderBook.Application.Simulations.Commands.SimulateBestPriceTrade;

namespace OrderBook.WebApi.Features.Simulations;

public record SimulateBestPriceTradeRequest(
    string Instrument,
    string Operation,
    decimal Quantity)
    : ICommand<SimulateBestPriceTradeResult>;
