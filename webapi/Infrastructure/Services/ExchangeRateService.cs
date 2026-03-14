using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SCISalesTest.Application.DTOs.ExternalServices;
using SCISalesTest.Application.ExternalServices;
using SCISalesTest.Domain.Exceptions;
using SCISalesTest.Domain.Options;
using SCISalesTest.Domain.Resources;

namespace SCISalesTest.Infrastructure.Services;

public class ExchangeRateService : IExchangeRateService
{
    private readonly HttpClient _httpClient;
    private readonly ExchangeRateOptions _options;
    private readonly ILogger<ExchangeRateService> _logger;
    private readonly IMessagesProvider _messagesProvider;

    public ExchangeRateService(
        IHttpClientFactory httpClientFactory,
        IOptions<ExchangeRateOptions> options,
        ILogger<ExchangeRateService> logger,
        IMessagesProvider messagesProvider)
    {
        _httpClient = httpClientFactory.CreateClient("ExchangeRate");
        _options = options.Value;
        _logger = logger;
        _messagesProvider = messagesProvider;
    }

    public async Task<decimal> GetExchangeRateAsync(string baseCurrency, string targetCurrency)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ExchangeRateResponseDto>(
                $"/v6/latest/{baseCurrency.ToUpperInvariant()}");

            if (response is null || response.Result != "success")
            {
                throw new ExternalServiceException(
                    string.Format(_messagesProvider.ExchangeRateServiceError, targetCurrency));
            }

            var upperTarget = targetCurrency.ToUpperInvariant();

            if (!response.Rates.TryGetValue(upperTarget, out var rate))
            {
                throw new ExternalServiceException(
                    string.Format(_messagesProvider.ExchangeRateServiceError, targetCurrency));
            }

            return rate;
        }
        catch (ExternalServiceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching exchange rate from {BaseCurrency} to {TargetCurrency}",
                baseCurrency, targetCurrency);

            throw new ExternalServiceException(_messagesProvider.ExternalServiceUnavailable, ex);
        }
    }

    public async Task<IEnumerable<string>> GetSupportedCurrenciesAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ExchangeRateResponseDto>(
                $"/v6/latest/{_options.DefaultBaseCurrency}");

            if (response is null || response.Result != "success")
            {
                throw new ExternalServiceException(_messagesProvider.ExternalServiceUnavailable);
            }

            return response.Rates.Keys.OrderBy(c => c);
        }
        catch (ExternalServiceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching supported currencies");
            throw new ExternalServiceException(_messagesProvider.ExternalServiceUnavailable, ex);
        }
    }
}
