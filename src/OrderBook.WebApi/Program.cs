using OrderBook.WebApi.Endpoints;
using OrderBook.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddOpenApi();
builder.Services.AddMongoDatabase(configuration);
builder.Services.AddApplicationHandlers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapTradeEndpoints();

app.Run();