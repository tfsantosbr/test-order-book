using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Options;
using OrderBook.Worker.BitstampClient;

namespace OrderBook.Worker;

public class Worker(ILogger<Worker> logger, IOptions<BitstampWebSocketSettings> options) : BackgroundService
{
    // Fields

    private readonly BitstampWebSocketSettings settings = options.Value;
    private readonly JsonSerializerOptions defaultJsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    private readonly JsonSerializerOptions jsonSerializerOptionsForLogging = new() { WriteIndented = true };

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
                logger.LogInformation("Connectio closed by server.");

                return;
            }

            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            DisplayFormattedJson(message);
        }
    }

    private void DisplayFormattedJson(string tradeString)
    {
        // try
        // {
        //     var parsedJson = JsonNode.Parse(tradeJson);
        //     var formattedJson = JsonSerializer.Serialize(parsedJson, new JsonSerializerOptions { WriteIndented = true });
        //     logger.LogInformation("{Json}", formattedJson);
        // }
        // catch (Exception ex)
        // {
        //     logger.LogError("Erro ao processar JSON: {Message}", ex.Message);
        // }

        try
        {
            var tradeParsedJson = JsonNode.Parse(tradeString);

            logger.LogInformation("JSON Parsed: {Json}", tradeParsedJson.ToJsonString());

            var tradeMessage = JsonSerializer.Deserialize<BitstampTradeResponse>(
                tradeParsedJson);

            logger.LogInformation("Event: {Event}, Channel: {Channel}, Trade ID: {Id}, Amount: {Amount}, Price: {Price}",
                tradeMessage.Event,
                tradeMessage.Channel,
                tradeMessage.Data.Id,
                tradeMessage.Data.Amount,
                tradeMessage.Data.Price
                );

            if (tradeMessage != null)
            {
                logger.LogInformation("{Trade}", JsonSerializer.Serialize(
                    tradeMessage, jsonSerializerOptionsForLogging));
            }
        }
        catch (Exception ex)
        {
            logger.LogError("Error processing JSON: {Message}", ex.Message);
        }
    }
}

