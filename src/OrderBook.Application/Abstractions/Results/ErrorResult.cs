namespace OrderBook.Application.Abstractions.Results;

public record ErrorResult : Result
{
    public ErrorResult(Error error) : base(error)
    {
    }

    public ErrorResult(Error[] errors) : base(errors)
    {
    }
}

public record ErrorResult<TData> : Result<TData>
{
    public ErrorResult(Error error) : base(error)
    {
    }

    public ErrorResult(Error[] errors) : base(errors)
    {
    }
}