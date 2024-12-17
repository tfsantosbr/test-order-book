using OrderBook.Application.Abstractions.Results;

namespace OrderBook.Application.Abstractions.Handlers;

public abstract class AbstractHandler
{
    protected static Result SuccessResult() =>
        Result.Success();

    protected static Result ErrorResult(Error[] notifications) =>
        Result.Error(notifications);

    protected static Result ErrorResult(Error notification) =>
        Result.Error(notification);

    protected static Result NotFoundResult(Error notification) =>
        Result.NotFound(notification);
}

public abstract class AbstractHandler<TData> where TData : class
{
    protected Result<TData> SuccessResult(TData data) =>
        Result<TData>.Success(data);

    protected Result<TData> ErrorResult(Error[] notifications) =>
        Result<TData>.Error(notifications);

    protected Result<TData> ErrorResult(Error notification) =>
        Result<TData>.Error(notification);

    protected Result<TData> NotFoundResult(Error notification) =>
        Result<TData>.NotFound(notification);
}
