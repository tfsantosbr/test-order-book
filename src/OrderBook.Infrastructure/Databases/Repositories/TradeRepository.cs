using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OrderBook.Application.Trades;
using OrderBook.Application.Trades.Repositories;
using OrderBook.Infrastructure.Databases.MongoDb;

namespace OrderBook.Infrastructure.Databases.Repositories;

public class TradeRepository : ITradeRepository
{
    private readonly IMongoCollection<Trade> _tradesCollection;

    public TradeRepository(IOptions<MongoDbSettings> mongoSettings)
    {
        var mongoClient = new MongoClient(
            mongoSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            mongoSettings.Value.DatabaseName);

        _tradesCollection = mongoDatabase.GetCollection<Trade>(
            mongoSettings.Value.TradesCollectionName);

    }

    public async Task AddAsync(Trade trade, CancellationToken cancellationToken = default) =>
        await _tradesCollection.InsertOneAsync(trade, cancellationToken: cancellationToken);
}
