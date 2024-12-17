using Microsoft.AspNetCore.Mvc;
using OrderBook.Application.Abstractions.Handlers;
using OrderBook.Application.Simulations.Commands.SimulateBestPriceTrade;
using OrderBook.WebApi.Extensions;

namespace OrderBook.WebApi.Features.Simulations;

public static class SimulationEndpoints
{
    public static void MapSimulationEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("v1/simulations", SimulateBestPriceTradeRequest)
            .Produces<SimulateBestPriceTradeResult>()
            .WithTags("Simulations");
    }

    public static async Task<IResult> SimulateBestPriceTradeRequest(
        [FromBody] SimulateBestPriceTradeRequest request,
        ICommandHandler<SimulateBestPriceTradeCommand, SimulateBestPriceTradeResult> commndHandler,
        CancellationToken cancellationToken = default)
    {
        var command = new SimulateBestPriceTradeCommand(request.Instrument, request.Operation, request.Quantity);

        var result = await commndHandler.HandleAsync(command, cancellationToken);

        return result.Created();
    }
}
