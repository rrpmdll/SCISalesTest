namespace SCISalesTest.Application.ExternalServices;

public interface IExchangeRateService
{
    Task<decimal> GetExchangeRateAsync(string baseCurrency, string targetCurrency);
    Task<IEnumerable<string>> GetSupportedCurrenciesAsync();
}
