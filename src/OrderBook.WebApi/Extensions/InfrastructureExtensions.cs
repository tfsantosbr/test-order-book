using MongoDB.Driver;
using OrderBook.Infrastructure.Databases.MongoDb;

namespace OrderBook.WebApi.Extensions;

public static class InfrastructureExtensions
{
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
