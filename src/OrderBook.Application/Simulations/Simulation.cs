﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OrderBook.Application.Trades;

namespace OrderBook.Application.Simulations;

public class Simulation
{
    public Simulation(string instrument, string operation, decimal requestedQuantity, decimal totalCost, IEnumerable<Trade> tradesUsed)
    {
        Id = Guid.NewGuid();
        Instrument = instrument;
        Operation = operation;
        RequestedQuantity = requestedQuantity;
        TotalCost = totalCost;
        TradesUsed = tradesUsed;
    }

    private Simulation()
    {
    }

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; private set; }
    public string Instrument { get; private set; } = string.Empty;
    public string Operation { get; private set; } = string.Empty;
    public decimal RequestedQuantity { get; private set; }
    public decimal TotalCost { get; private set; }
    public IEnumerable<Trade> TradesUsed { get; private set; } = default!;
}