using OrderBook.Application.Trades.Repositories;
using OrderBook.Infrastructure.Databases.MongoDb;
using OrderBook.Infrastructure.Databases.Repositories;
using OrderBook.Worker.BitstampClient;
using OrderBook.Worker.Workers;

var builder = Host.CreateApplicationBuilder(args);
var configuration = builder.Configuration;

builder.Services.Configure<BitstampWebSocketSettings>(configuration.GetSection("BitstampWebSocketSettings"));
builder.Services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));

builder.Services.AddTransient<ITradeRepository, TradeRepository>();

builder.Services.AddHostedService<BitstampTradeListener>();

var host = builder.Build();
host.Run();
