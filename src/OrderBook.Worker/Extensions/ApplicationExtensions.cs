using OrderBook.Application.Abstractions.Handlers;
using OrderBook.Application.Trades.Queries.GetTradeStatistics;

namespace OrderBook.Worker.Extensions;

public static class ApplicationExtensions
{
    public static void AddApplicationHandlers(this IServiceCollection services)
    {
        services.AddTransient<IQueryHandler<GetTradeStatisticsQuery, GetTradeStatisticsQueryResult>, GetTradeStatisticsQueryHandler>();
    }
}
