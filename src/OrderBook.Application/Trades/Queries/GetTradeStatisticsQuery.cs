using OrderBook.Application.Abstractions.Handlers;
using OrderBook.Application.Trades.Models;

namespace OrderBook.Application.Trades.Queries;

public record GetTradeStatisticsQuery : IQuery<TradeStatisticsModel>;
