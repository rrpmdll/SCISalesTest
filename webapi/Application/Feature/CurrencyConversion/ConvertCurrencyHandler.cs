using MediatR;
using SCISalesTest.Application.DTOs.ExternalServices;
using SCISalesTest.Application.ExternalServices;

namespace SCISalesTest.Application.Feature.CurrencyConversion;

public class ConvertCurrencyHandler : 
    IRequestHandler<ConvertCurrencyQuery, CurrencyConversionDto>
{
    private readonly IExchangeRateService _exchangeRateService;

    public ConvertCurrencyHandler(
        IExchangeRateService exchangeRateService
    )
    {
        _exchangeRateService = exchangeRateService;
    }

    public async Task<CurrencyConversionDto> Handle(
        ConvertCurrencyQuery request, 
        CancellationToken cancellationToken
    )
    {
        var exchangeRate = await _exchangeRateService.GetExchangeRateAsync(
            request.SourceCurrency, 
            request.TargetCurrency
        );

        return new CurrencyConversionDto
        {
            Amount = request.Amount,
            SourceCurrency = request.SourceCurrency.ToUpperInvariant(),
            TargetCurrency = request.TargetCurrency.ToUpperInvariant(),
            ExchangeRate = exchangeRate,
            ConvertedAmount = Math.Round(request.Amount * exchangeRate, 2)
        };
    }
}
