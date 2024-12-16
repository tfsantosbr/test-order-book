using OrderBook.Application.Abstractions.Handlers;
using OrderBook.Application.Trades.Models;
using OrderBook.Application.Trades.Queries;

namespace OrderBook.WebApi.Endpoints;

public static class TradeEndpoints
{
    public static void MapTradeEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("v1/trades/statistics", GetTradeStatisticsQuery)
            .Produces<TradeStatisticsModel>()
            .WithTags("Trades");
    }

    public static async Task<IResult> GetTradeStatisticsQuery(
        IQueryHandler<GetTradeStatisticsQuery, TradeStatisticsModel> queryHandler,
        CancellationToken cancellationToken = default)
    {
        var result = await queryHandler.HandleAsync(new GetTradeStatisticsQuery(), cancellationToken);

        return Results.Ok(result);
    }
}
