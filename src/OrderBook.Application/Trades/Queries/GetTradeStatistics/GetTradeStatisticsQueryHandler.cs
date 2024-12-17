using MongoDB.Bson;
using MongoDB.Driver;
using OrderBook.Application.Abstractions.Handlers;
using OrderBook.Application.Abstractions.Results;

namespace OrderBook.Application.Trades.Queries.GetTradeStatistics;

public class GetTradeStatisticsQueryHandler(IMongoDatabase mongoDatabase) : IQueryHandler<GetTradeStatisticsQuery, GetTradeStatisticsQueryResult>
{
    private readonly IMongoCollection<Trade> _tradesCollection = mongoDatabase.GetCollection<Trade>("Trades");

    public async Task<Result<GetTradeStatisticsQueryResult>> HandleAsync(GetTradeStatisticsQuery query, CancellationToken cancellationToken = default)
    {
        var fiveSecondsAgo = DateTimeOffset.UtcNow.AddSeconds(-5).ToUnixTimeSeconds();

        var pipeline = _tradesCollection.Aggregate()
            .Group(trade => trade.Channel, g => new
            {
                Channel = g.Key,
                MaxPrice = g.Max(t => t.Price),
                MinPrice = g.Min(t => t.Price),
                AveragePrice = g.Average(t => t.Price),
                AverageAmount = g.Average(t => t.Amount),
                TradesInLast5Sec = g.Where(t => t.Timestamp >= fiveSecondsAgo).Select(t => t.Price)
            })
            .Project(g => new GetTradeStatisticsQueryResultItem
            {
                Channel = g.Channel,
                MaxPrice = g.MaxPrice,
                MinPrice = g.MinPrice,
                AveragePrice = g.AveragePrice,
                AverageAmount = g.AverageAmount,
                AveragePriceLast5Sec = g.TradesInLast5Sec.Any() ? g.TradesInLast5Sec.Average() : 0
            });

        var tradeStatisticsModelItems = await pipeline.ToListAsync(cancellationToken);
        var tradeStatisticsModel = new GetTradeStatisticsQueryResult { TradeStats = tradeStatisticsModelItems };

        return Result.Success(tradeStatisticsModel);
    }
}