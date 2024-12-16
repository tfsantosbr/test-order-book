namespace OrderBook.Application.Trades.Models;

public record TradeStatisticsModel
{
    public List<TradeStatisticsModelItem> TradeStats { get; set; } = [];
}

public record TradeStatisticsModelItem
{
    public string Channel { get; set; } = string.Empty;
    public decimal MaxPrice { get; set; }
    public decimal MinPrice { get; set; }
    public decimal AveragePrice { get; set; }
    public decimal AverageAmount { get; set; }
    public decimal AveragePriceLast5Sec { get; set; }
}
