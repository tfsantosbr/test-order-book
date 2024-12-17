using OrderBook.Application.Abstractions.Handlers;
using OrderBook.Application.Trades.Queries.GetTradeStatistics;
using System.Text.Json;

namespace OrderBook.Worker.Workers;

public class GetTradeStatisticsListener(
    ILogger<BitstampTradeListener> logger, 
    IQueryHandler<GetTradeStatisticsQuery, GetTradeStatisticsQueryResult> queryHandler) : BackgroundService
{
    // Fields

    private readonly JsonSerializerOptions defaultJsonSerializerOptions = new() 
    { 
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase, 
        WriteIndented = true 
    };

    // Implementations

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var result = await queryHandler.HandleAsync(new GetTradeStatisticsQuery(), stoppingToken);
            var resultJson = JsonSerializer.Serialize(result.Data!, defaultJsonSerializerOptions);

            logger.LogInformation("Trade Statistics: {TradeStatisticsJson}", resultJson);

            await Task.Delay(5000, stoppingToken);
        }
    }
}

