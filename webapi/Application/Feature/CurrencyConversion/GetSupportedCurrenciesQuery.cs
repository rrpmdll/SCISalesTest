using MediatR;

namespace SCISalesTest.Application.Feature.CurrencyConversion;

public record GetSupportedCurrenciesQuery : IRequest<IEnumerable<string>>;
