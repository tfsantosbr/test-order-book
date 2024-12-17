namespace OrderBook.Application.Abstractions.Results;

public record SuccessResult : Result;

public record SuccessResult<TData>(TData data) : Result<TData>(data);