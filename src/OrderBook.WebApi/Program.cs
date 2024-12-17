using OrderBook.WebApi.Extensions;
using OrderBook.WebApi.Features.Simulations;
using OrderBook.WebApi.Features.Trades;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddOpenApi();
builder.Services.AddMongoDatabase(configuration);
builder.Services.AddRepositories();
builder.Services.AddApplicationHandlers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapTradeEndpoints();
app.MapSimulationEndpoints();

app.Run();