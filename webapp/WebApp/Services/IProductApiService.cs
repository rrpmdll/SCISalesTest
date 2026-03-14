using SCISalesTest.WebApp.Models;

namespace SCISalesTest.WebApp.Services;

public interface IProductApiService
{
    Task<IEnumerable<ProductViewModel>> GetAllAsync();
    Task<ProductViewModel?> GetByIdAsync(int id);
    Task<ProductViewModel?> CreateAsync(CreateProductViewModel model);
    Task<ProductViewModel?> UpdateAsync(EditProductViewModel model);
    Task<bool> DeleteAsync(int id);
    Task<ProductExchangeRateViewModel?> GetWithExchangeRateAsync(int id, string targetCurrency);
    Task<IEnumerable<ProductExchangeRateViewModel>> GetAllWithExchangeRateAsync(string targetCurrency);
    Task<CurrencyConverterViewModel?> ConvertCurrencyAsync(decimal amount, string sourceCurrency, string targetCurrency);
    Task<List<string>> GetSupportedCurrenciesAsync();
}
