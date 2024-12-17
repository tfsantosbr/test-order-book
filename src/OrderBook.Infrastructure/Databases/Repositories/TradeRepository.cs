using MongoDB.Driver;
using OrderBook.Application.Trades;
using OrderBook.Application.Trades.Repositories;

namespace OrderBook.Infrastructure.Databases.Repositories;

public class TradeRepository(IMongoDatabase mongoDatabase) : ITradeRepository
{
    private readonly IMongoCollection<Trade> _tradesCollection = mongoDatabase.GetCollection<Trade>("Trades");

    public async Task AddAsync(Trade trade, CancellationToken cancellationToken = default) =>
        await _tradesCollection.InsertOneAsync(trade, cancellationToken: cancellationToken);

    public async Task<IEnumerable<Trade>> GetTradesByInstrumentForBuyAsync(string instrument, CancellationToken cancellationToken)
    {
        return await _tradesCollection
            .Find(t => t.Channel == instrument && t.TradeType == 0)
            .SortByDescending(t => t.Price)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Trade>> GetTradesByInstrumentForSellAsync(string instrument, CancellationToken cancellationToken)
    {
        return await _tradesCollection
            .Find(t => t.Channel == instrument && t.TradeType == 1)
            .SortBy(t => t.Price)
            .ToListAsync(cancellationToken);
    }
}
