using OrderBook.Application.Abstractions.Handlers;
using OrderBook.Application.Trades.Queries.GetTradeStatistics;

namespace OrderBook.WebApi.Features.Trades;

public static class TradeEndpoints
{
    public static void MapTradeEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("v1/trades/statistics", GetTradeStatisticsQuery)
            .Produces<GetTradeStatisticsQueryResult>()
            .WithTags("Trades");
    }

    public static async Task<IResult> GetTradeStatisticsQuery(
        IQueryHandler<GetTradeStatisticsQuery, GetTradeStatisticsQueryResult> queryHandler,
        CancellationToken cancellationToken = default)
    {
        var result = await queryHandler.HandleAsync(new GetTradeStatisticsQuery(), cancellationToken);

        return Results.Ok(result);
    }
}
