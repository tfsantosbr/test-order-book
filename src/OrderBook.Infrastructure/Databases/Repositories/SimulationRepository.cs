using MongoDB.Driver;
using OrderBook.Application.Simulations;
using OrderBook.Application.Simulations.Repositories;

namespace OrderBook.Infrastructure.Databases.Repositories;

public class SimulationRepository(IMongoDatabase mongoDatabase) : ISimulationRepository
{
    private readonly IMongoCollection<Simulation> _simulationCollection = mongoDatabase.GetCollection<Simulation>("Simulations");
    
    public Task Add(Simulation simulation, CancellationToken cancellationToken = default) =>
        _simulationCollection.InsertOneAsync(simulation, cancellationToken: cancellationToken);
}
