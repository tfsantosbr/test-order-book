using OrderBook.Application.Abstractions.Results;

namespace OrderBook.WebApi.Extensions;

public static class ResultExtensions
{
    // Extension Methods

    public static IResult Ok<TValue>(this Result<TValue> result) =>
        BaseResult(result, TypedResults.Ok(result.Data));

    public static IResult NoContent(this Result result) =>
        BaseResult(result, TypedResults.NoContent());

    public static IResult Accepted(this Result result, string? uri = null) =>
        BaseResult(result, TypedResults.Accepted(uri));

    public static IResult Created<TValue>(this Result<TValue> result, string? uri = null) =>
        BaseResult(result, TypedResults.Created(uri, result.Data));

    // Private Methods

    private static IResult BaseResult(Result result, IResult successResult) =>
        result switch
        {
            NotFoundResult => TypedResults.NotFound(result.Notifications),
            ErrorResult => TypedResults.BadRequest(result.Notifications),
            SuccessResult => successResult,
            _ => throw new ArgumentOutOfRangeException(
                nameof(result), $"Unexpected result type: {result.GetType().Name}"),
        };

    private static IResult BaseResult<TValue>(Result<TValue> result, IResult successResult) =>
        result switch
        {
            NotFoundResult<TValue> => TypedResults.NotFound(result.Notifications),
            ErrorResult<TValue> => TypedResults.BadRequest(result.Notifications),
            SuccessResult<TValue> => successResult,
            _ => throw new ArgumentOutOfRangeException(
                nameof(result), $"Unexpected result type: {result.GetType().Name}"),
        };
}
