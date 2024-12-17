namespace OrderBook.Application.Simulations.Repositories;

public interface ISimulationRepository
{
    Task Add(Simulation simulation, CancellationToken cancellationToken = default);
}
