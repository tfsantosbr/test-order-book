namespace OrderBook.Application.Abstractions.Results;

public abstract record Result
{
    protected Result()
    {
    }

    protected Result(Error notification) =>
        Notifications = [notification];

    protected Result(Error[] notifications) =>
        Notifications = notifications;

    public Error[] Notifications { get; } = [];
    public bool IsSuccess => Notifications.Length == 0;
    public bool IsFailure => !IsSuccess;

    public static ErrorResult Error(Error notification) => new(notification);
    public static ErrorResult Error(Error[] notifications) => new(notifications);
    public static NotFoundResult NotFound(Error notification) => new(notification);
    public static SuccessResult Success() => new();
    public static SuccessResult<TData> Success<TData>(TData data) => new(data);
}

public abstract record Result<TData> : Result
{
    public TData? Data { get; }

    internal Result(TData data)
    {
        Data = data;
    }

    internal Result(Error notification)
        : base(notification)
    {
    }

    internal Result(Error[] notifications)
        : base(notifications)
    {
    }

    public static new ErrorResult<TData> Error(Error notification) => new(notification);
    public static new ErrorResult<TData> Error(Error[] notifications) => new(notifications);
    public static new NotFoundResult<TData> NotFound(Error notification) => new(notification);
    public static SuccessResult<TData> Success(TData data) => new(data);
}