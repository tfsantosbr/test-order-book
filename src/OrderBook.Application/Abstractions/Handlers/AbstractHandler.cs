using OrderBook.Application.Abstractions.Results;

namespace OrderBook.Application.Abstractions.Handlers;

public abstract class AbstractHandler
{
    protected static Result SuccessResult() =>
        Result.Success();

    protected static Result ErrorResult(Error[] errors) =>
        Result.Error(errors);

    protected static Result ErrorResult(Error error) =>
        Result.Error(error);

    protected static Result NotFoundResult(Error error) =>
        Result.NotFound(error);
}

public abstract class AbstractHandler<TData> where TData : class
{
    protected Result<TData> SuccessResult(TData data) =>
        Result<TData>.Success(data);

    protected Result<TData> ErrorResult(Error[] errors) =>
        Result<TData>.Error(errors);

    protected Result<TData> ErrorResult(Error error) =>
        Result<TData>.Error(error);

    protected Result<TData> NotFoundResult(Error error) =>
        Result<TData>.NotFound(error);
}
