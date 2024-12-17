namespace OrderBook.Application.Trades.Repositories;

public interface ITradeRepository
{
    Task AddAsync(Trade trade, CancellationToken cancellationToken = default);
    Task<IEnumerable<Trade>> GetTradesByInstrumentForBuyAsync(string instrument, CancellationToken cancellationToken);
    Task<IEnumerable<Trade>> GetTradesByInstrumentForSellAsync(string instrument, CancellationToken cancellationToken);

}
