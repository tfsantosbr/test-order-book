namespace OrderBook.Application.Abstractions.Handlers;

public interface ICommand : IBaseCommand;

public interface ICommand<TResponse> : IBaseCommand;

public interface IBaseCommand;
