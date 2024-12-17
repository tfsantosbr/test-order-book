namespace OrderBook.Application.Trades.Queries.GetTradeStatistics;

public record GetTradeStatisticsQueryResult
{
    public List<GetTradeStatisticsQueryResultItem> TradeStats { get; set; } = [];
}

public record GetTradeStatisticsQueryResultItem
{
    public string Channel { get; set; } = string.Empty;
    public decimal MaxPrice { get; set; }
    public decimal MinPrice { get; set; }
    public decimal AveragePrice { get; set; }
    public decimal AverageAmount { get; set; }
    public decimal AveragePriceLast5Sec { get; set; }
}
