using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OrderBook.Application.Trades;
using OrderBook.Application.Trades.Repositories;
using OrderBook.Infrastructure.Databases.MongoDb;

namespace OrderBook.Infrastructure.Databases.Repositories;

public class TradeRepository(IMongoDatabase mongoDatabase) : ITradeRepository
{
    private readonly IMongoCollection<Trade> _tradesCollection = mongoDatabase.GetCollection<Trade>("Trades");

    public async Task AddAsync(Trade trade, CancellationToken cancellationToken = default) =>
        await _tradesCollection.InsertOneAsync(trade, cancellationToken: cancellationToken);
}
