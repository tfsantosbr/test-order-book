using Microsoft.Extensions.Options;
using OrderBook.Application.Trades;
using OrderBook.Application.Trades.Repositories;
using OrderBook.Worker.BitstampClient;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace OrderBook.Worker.Workers;

public class BitstampTradeListener(
    ILogger<BitstampTradeListener> logger, IOptions<BitstampWebSocketSettings> options,
    ITradeRepository tradeRepository) : BackgroundService
{
    // Fields

    private readonly BitstampWebSocketSettings settings = options.Value;
    private readonly JsonSerializerOptions defaultJsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    // Implementations

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var clientWebSocket = new ClientWebSocket();

            try
            {
                logger.LogInformation("Connection to Bitstamp WebSocket on {Url}...", settings.Url);
                await clientWebSocket.ConnectAsync(new Uri(settings.Url), stoppingToken);
                logger.LogInformation("Connection established.");

                foreach (var instrument in settings.Instruments)
                    await SubscribeToChannel(clientWebSocket, instrument, stoppingToken);

                await ReceiveMessages(clientWebSocket, stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError("Error establishing connection: {Message}", ex.Message);
            }
        }
    }

    // Private Methods

    private async Task SubscribeToChannel(
        ClientWebSocket clientWebSocket, string channel, CancellationToken stoppingToken)
    {
        var subscribeRequest = new BitstampSubscribeToChannelRequest(
            Event: "bts:subscribe",
            Data: new BitstampSubscribeToChannelRequestData(
                Channel: channel
            )
        );

        var jsonRequest = JsonSerializer.Serialize(subscribeRequest, defaultJsonSerializerOptions);
        var messageBuffer = Encoding.UTF8.GetBytes(jsonRequest);

        await clientWebSocket.SendAsync(
            new ArraySegment<byte>(messageBuffer), WebSocketMessageType.Text, true, stoppingToken);

        logger.LogInformation("Subscribed to channel '{Channel}'.", channel);
    }

    private async Task ReceiveMessages(ClientWebSocket clientWebSocket, CancellationToken stoppingToken)
    {
        int defaultBufgfersize = 8192;
        var buffer = new byte[defaultBufgfersize];

        while (clientWebSocket.State == WebSocketState.Open && !stoppingToken.IsCancellationRequested)
        {
            var result = await clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), stoppingToken);

            if (result.MessageType == WebSocketMessageType.Close)
            {
                await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, stoppingToken);
                logger.LogInformation("Connection closed by server.");

                return;
            }

            var tradeString = Encoding.UTF8.GetString(buffer, 0, result.Count);

            try
            {
                var tradeMessage = JsonSerializer.Deserialize<BitstampTradeResponse>(tradeString);

                if (tradeMessage != null && IsTradeEvent(tradeMessage.Event))
                {
                    await SaveMessageInDatabaseAsync(tradeMessage, stoppingToken);

                    logger.LogInformation("Trade {TradeId} saved in database with success", tradeMessage.Data.Id);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error processing JSON: {Message}", ex.Message);

                throw;
            }


        }
    }

    private async Task SaveMessageInDatabaseAsync(BitstampTradeResponse tradeMessage, CancellationToken stoppingToken)
    {
        var trade = new Trade(
            id: tradeMessage.Data.Id,
            channel: tradeMessage.Channel,
            @event: tradeMessage.Event,
            timestamp: long.Parse(tradeMessage.Data.Timestamp),
            amount: tradeMessage.Data.Amount,
            amountStr: tradeMessage.Data.AmountStr,
            price: tradeMessage.Data.Price,
            priceStr: tradeMessage.Data.PriceStr,
            type: tradeMessage.Data.Type,
            microtimestamp: long.Parse(tradeMessage.Data.Microtimestamp),
            buyOrderId: tradeMessage.Data.BuyOrderId,
            sellOrderId: tradeMessage.Data.SellOrderId
        );

        await tradeRepository.AddAsync(trade, stoppingToken);
    }

    private static bool IsTradeEvent(string tradeEvent) => tradeEvent == "trade";
}

