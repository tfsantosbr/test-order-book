using OrderBook.Application.Abstractions.Handlers;
using OrderBook.Application.Trades.Models;
using OrderBook.Application.Trades.Queries;

namespace OrderBook.WebApi.Extensions;

public static class ApplicationExtensions
{
    public static void AddApplicationHandlers(this IServiceCollection services)
    {
        services.AddTransient<IQueryHandler<GetTradeStatisticsQuery, TradeStatisticsModel>, GetTradeStatisticsQueryHandler>();
    }
}
