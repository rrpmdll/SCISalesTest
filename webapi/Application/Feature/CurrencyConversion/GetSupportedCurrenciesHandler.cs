using MediatR;
using SCISalesTest.Application.ExternalServices;

namespace SCISalesTest.Application.Feature.CurrencyConversion;

public class GetSupportedCurrenciesHandler : 
    IRequestHandler<GetSupportedCurrenciesQuery, IEnumerable<string>>
{
    private readonly IExchangeRateService _exchangeRateService;

    public GetSupportedCurrenciesHandler(IExchangeRateService exchangeRateService)
    {
        _exchangeRateService = exchangeRateService;
    }

    public async Task<IEnumerable<string>> Handle(
        GetSupportedCurrenciesQuery request, 
        CancellationToken cancellationToken
    ) => await _exchangeRateService
        .GetSupportedCurrenciesAsync();
}
