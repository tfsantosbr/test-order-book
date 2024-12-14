namespace OrderBook.Worker.BitstampClient;

public record BitstampSubscribeToChannelRequest(
    string Event,
    BitstampSubscribeToChannelRequestData Data);

public record BitstampSubscribeToChannelRequestData(
    string Channel);
