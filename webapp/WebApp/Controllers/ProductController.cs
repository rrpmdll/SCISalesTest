using Microsoft.AspNetCore.Mvc;
using SCISalesTest.WebApp.Models;
using SCISalesTest.WebApp.Services;

namespace SCISalesTest.WebApp.Controllers;

public class ProductController : Controller
{
    private readonly IProductApiService _productApiService;

    public ProductController(IProductApiService productApiService)
    {
        _productApiService = productApiService;
    }

    public async Task<IActionResult> Index(string? currency)
    {
        var selectedCurrency = currency ?? "COP";
        var products = await _productApiService.GetAllWithExchangeRateAsync(selectedCurrency);
        var supportedCurrencies = await _productApiService.GetSupportedCurrenciesAsync();

        var viewModel = new ProductListViewModel
        {
            Products = products,
            SelectedCurrency = selectedCurrency,
            SupportedCurrencies = supportedCurrencies
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _productApiService.GetByIdAsync(id);
        if (product is null)
            return NotFound();

        return View(product);
    }

    public IActionResult Create()
    {
        return View(new CreateProductViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProductViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _productApiService.CreateAsync(model);
        if (result is null)
        {
            ModelState.AddModelError(string.Empty, "An error occurred while creating the product.");
            return View(model);
        }

        TempData["Success"] = "Product created successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productApiService.GetByIdAsync(id);
        if (product is null)
            return NotFound();

        var editModel = new EditProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            UnitsInStock = product.UnitsInStock,
            IsActive = product.IsActive
        };

        return View(editModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditProductViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _productApiService.UpdateAsync(model);
        if (result is null)
        {
            ModelState.AddModelError(string.Empty, "An error occurred while updating the product.");
            return View(model);
        }

        TempData["Success"] = "Product updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productApiService.GetByIdAsync(id);
        if (product is null)
            return NotFound();

        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var success = await _productApiService.DeleteAsync(id);
        if (!success)
        {
            TempData["Error"] = "An error occurred while deleting the product.";
            return RedirectToAction(nameof(Index));
        }

        TempData["Success"] = "Product deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> ExchangeRate(int id, string? targetCurrency)
    {
        if (string.IsNullOrWhiteSpace(targetCurrency))
        {
            var product = await _productApiService.GetByIdAsync(id);
            if (product is null)
                return NotFound();

            return View(new ProductExchangeRateViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                OriginalPrice = product.Price,
                OriginalCurrency = "USD"
            });
        }

        var result = await _productApiService.GetWithExchangeRateAsync(id, targetCurrency);
        if (result is null)
        {
            TempData["Error"] = $"Could not retrieve exchange rate for currency '{targetCurrency}'.";
            return RedirectToAction(nameof(Details), new { id });
        }

        return View(result);
    }
}
