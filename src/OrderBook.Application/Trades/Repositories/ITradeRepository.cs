namespace OrderBook.Application.Trades.Repositories;

public interface ITradeRepository
{
    Task AddAsync(Trade trade, CancellationToken cancellationToken = default);
}
