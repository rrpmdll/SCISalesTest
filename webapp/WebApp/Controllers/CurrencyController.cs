using Microsoft.AspNetCore.Mvc;
using SCISalesTest.WebApp.Models;
using SCISalesTest.WebApp.Services;

namespace SCISalesTest.WebApp.Controllers;

public class CurrencyController : Controller
{
    private readonly IProductApiService _productApiService;

    public CurrencyController(IProductApiService productApiService)
    {
        _productApiService = productApiService;
    }

    public async Task<IActionResult> Index()
    {
        var currencies = await _productApiService.GetSupportedCurrenciesAsync();
        return View(new CurrencyConverterViewModel { SupportedCurrencies = currencies });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(CurrencyConverterViewModel model)
    {
        model.SupportedCurrencies = await _productApiService.GetSupportedCurrenciesAsync();

        if (!ModelState.IsValid)
            return View(model);

        var result = await _productApiService.ConvertCurrencyAsync(
            model.Amount, model.SourceCurrency, model.TargetCurrency);

        if (result is null)
        {
            TempData["Error"] = "Could not perform currency conversion. Please try again.";
            return View(model);
        }

        result.SupportedCurrencies = model.SupportedCurrencies;
        return View(result);
    }
}
