using SCISalesTest.WebApp.Models;

namespace SCISalesTest.WebApp.Services;

public class ProductApiService : IProductApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProductApiService> _logger;

    public ProductApiService(HttpClient httpClient, ILogger<ProductApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IEnumerable<ProductViewModel>> GetAllAsync()
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<IEnumerable<ProductViewModel>>("api/product");
            return result ?? Enumerable.Empty<ProductViewModel>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all products from API");
            return Enumerable.Empty<ProductViewModel>();
        }
    }

    public async Task<ProductViewModel?> GetByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<ProductViewModel>($"api/product/{id}");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching product {ProductId} from API", id);
            return null;
        }
    }

    public async Task<ProductViewModel?> CreateAsync(CreateProductViewModel model)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/product", new
            {
                model.Name,
                model.Description,
                model.Price,
                model.UnitsInStock
            });

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProductViewModel>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product via API");
            return null;
        }
    }

    public async Task<ProductViewModel?> UpdateAsync(EditProductViewModel model)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/product/{model.Id}", new
            {
                model.Id,
                model.Name,
                model.Description,
                model.Price,
                model.UnitsInStock,
                model.IsActive
            });

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProductViewModel>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product {ProductId} via API", model.Id);
            return null;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/product/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product {ProductId} via API", id);
            return false;
        }
    }

    public async Task<ProductExchangeRateViewModel?> GetWithExchangeRateAsync(int id, string targetCurrency)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<ProductExchangeRateViewModel>(
                $"api/product/{id}/exchange-rate?targetCurrency={Uri.EscapeDataString(targetCurrency)}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching exchange rate for product {ProductId} to {Currency}", id, targetCurrency);
            return null;
        }
    }

    public async Task<IEnumerable<ProductExchangeRateViewModel>> GetAllWithExchangeRateAsync(string targetCurrency)
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<IEnumerable<ProductExchangeRateViewModel>>(
                $"api/product/exchange-rate?targetCurrency={Uri.EscapeDataString(targetCurrency)}");
            return result ?? Enumerable.Empty<ProductExchangeRateViewModel>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all products with exchange rate to {Currency}", targetCurrency);
            return Enumerable.Empty<ProductExchangeRateViewModel>();
        }
    }

    public async Task<CurrencyConverterViewModel?> ConvertCurrencyAsync(decimal amount, string sourceCurrency, string targetCurrency)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<CurrencyConversionApiResponse>(
                $"api/currency/convert?amount={amount}&sourceCurrency={Uri.EscapeDataString(sourceCurrency)}&targetCurrency={Uri.EscapeDataString(targetCurrency)}");

            if (response is null) return null;

            return new CurrencyConverterViewModel
            {
                Amount = response.Amount,
                SourceCurrency = response.SourceCurrency,
                TargetCurrency = response.TargetCurrency,
                ExchangeRate = response.ExchangeRate,
                ConvertedAmount = response.ConvertedAmount
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error converting {Amount} from {Source} to {Target}", amount, sourceCurrency, targetCurrency);
            return null;
        }
    }

    private class CurrencyConversionApiResponse
    {
        public decimal Amount { get; set; }
        public string SourceCurrency { get; set; } = string.Empty;
        public string TargetCurrency { get; set; } = string.Empty;
        public decimal ExchangeRate { get; set; }
        public decimal ConvertedAmount { get; set; }
    }

    public async Task<List<string>> GetSupportedCurrenciesAsync()
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<List<string>>("api/currency/supported-currencies");
            return result ?? new List<string>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching supported currencies from API");
            return new List<string>();
        }
    }
}
