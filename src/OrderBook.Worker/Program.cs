using OrderBook.Worker.BitstampClient;
using OrderBook.Worker.Extensions;

var builder = Host.CreateApplicationBuilder(args);
var configuration = builder.Configuration;

builder.Services.Configure<BitstampWebSocketSettings>(configuration.GetSection("BitstampWebSocketSettings"));

builder.Services.AddMongoDatabase(configuration);
builder.Services.AddRepositories();
builder.Services.AddApplicationHandlers();
builder.Services.AddListeners();

var host = builder.Build();
host.Run();
