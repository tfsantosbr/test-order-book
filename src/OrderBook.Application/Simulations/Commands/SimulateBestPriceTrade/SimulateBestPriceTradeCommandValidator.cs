using FluentValidation;

namespace OrderBook.Application.Simulations.Commands.SimulateBestPriceTrade;

public class SimulateBestPriceTradeCommandValidator : AbstractValidator<SimulateBestPriceTradeCommand>
{
    public SimulateBestPriceTradeCommandValidator()
    {
        RuleFor(x => x.Instrument)
            .NotEmpty()
            .Must(instrument => instrument == "btcusd" || instrument == "ethusd")
                .WithMessage("Instrument must be either 'btcusd' or 'ethusd'.");

        RuleFor(x => x.Operation).IsInEnum();
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}
