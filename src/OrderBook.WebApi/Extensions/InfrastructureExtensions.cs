using MongoDB.Driver;
using OrderBook.Application.Simulations.Repositories;
using OrderBook.Application.Trades.Repositories;
using OrderBook.Infrastructure.Databases.MongoDb;
using OrderBook.Infrastructure.Databases.Repositories;

namespace OrderBook.WebApi.Extensions;

public static class InfrastructureExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<ITradeRepository, TradeRepository>();
        services.AddTransient<ISimulationRepository, SimulationRepository>();
    }

    public static void AddMongoDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(provider =>
        {
            var mongoSettings = configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();

            ArgumentNullException.ThrowIfNull(mongoSettings, nameof(mongoSettings));

            var mongoClient = new MongoClient(mongoSettings.ConnectionString);

            return mongoClient.GetDatabase(mongoSettings.DatabaseName);
        });
    }
}
