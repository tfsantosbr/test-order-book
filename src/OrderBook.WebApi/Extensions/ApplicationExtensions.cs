using FluentValidation;
using OrderBook.Application.Abstractions.Handlers;
using OrderBook.Application.Simulations.Commands.SimulateBestPriceTrade;
using OrderBook.Application.Trades.Queries.GetTradeStatistics;

namespace OrderBook.WebApi.Extensions;

public static class ApplicationExtensions
{
    public static void AddApplicationHandlers(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<SimulateBestPriceTradeCommandValidator>();
        services.AddTransient<IQueryHandler<GetTradeStatisticsQuery, GetTradeStatisticsQueryResult>, GetTradeStatisticsQueryHandler>();
        services.AddTransient<ICommandHandler<SimulateBestPriceTradeCommand, SimulateBestPriceTradeResult>, SimulateBestPriceTradeCommandHandler>();
    }
}
