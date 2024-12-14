using OrderBook.Worker;
using OrderBook.Worker.BitstampClient;

var builder = Host.CreateApplicationBuilder(args);
var configuration = builder.Configuration;

builder.Services.Configure<BitstampWebSocketSettings>(configuration.GetSection("BitstampWebSocketSettings"));
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
