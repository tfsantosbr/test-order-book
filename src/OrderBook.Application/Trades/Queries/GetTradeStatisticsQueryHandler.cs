using MongoDB.Bson;
using MongoDB.Driver;
using OrderBook.Application.Abstractions.Handlers;
using OrderBook.Application.Trades.Models;

namespace OrderBook.Application.Trades.Queries;

public class GetTradeStatisticsQueryHandler(IMongoDatabase mongoDatabase) : IQueryHandler<GetTradeStatisticsQuery, TradeStatisticsModel>
{
    private readonly IMongoCollection<Trade> _tradesCollection = mongoDatabase.GetCollection<Trade>("Trades");

    public async Task<TradeStatisticsModel> HandleAsync(GetTradeStatisticsQuery query, CancellationToken cancellationToken = default)
    {
        var currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var fiveSecondsAgo = currentTime - 5000;

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
            .Project(g => new TradeStatisticsModelItem
            {
                Channel = g.Channel,
                MaxPrice = g.MaxPrice,
                MinPrice = g.MinPrice,
                AveragePrice = g.AveragePrice,
                AverageAmount = g.AverageAmount,
                AveragePriceLast5Sec = g.TradesInLast5Sec.Any() ? g.TradesInLast5Sec.Average() : 0
            });

        var tradeStatisticsModelItems = await pipeline.ToListAsync(cancellationToken);

        return new TradeStatisticsModel { TradeStats = tradeStatisticsModelItems };
    }
}