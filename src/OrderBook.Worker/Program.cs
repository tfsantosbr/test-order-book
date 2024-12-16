using MongoDB.Driver;
using OrderBook.Application.Trades.Repositories;
using OrderBook.Infrastructure.Databases.MongoDb;
using OrderBook.Infrastructure.Databases.Repositories;
using OrderBook.Worker.BitstampClient;
using OrderBook.Worker.Workers;

var builder = Host.CreateApplicationBuilder(args);
var configuration = builder.Configuration;

builder.Services.Configure<BitstampWebSocketSettings>(configuration.GetSection("BitstampWebSocketSettings"));

builder.Services.AddSingleton(provider =>
{
    var mongoSettings = configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();

    ArgumentNullException.ThrowIfNull(mongoSettings, nameof(mongoSettings));

    var mongoClient = new MongoClient(mongoSettings.ConnectionString);

    return mongoClient.GetDatabase(mongoSettings.DatabaseName);
});

builder.Services.AddTransient<ITradeRepository, TradeRepository>();

builder.Services.AddHostedService<BitstampTradeListener>();

var host = builder.Build();
host.Run();
