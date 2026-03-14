using System.ComponentModel.DataAnnotations;

namespace SCISalesTest.WebApp.Models;

public class ProductViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int UnitsInStock { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}

public class CreateProductViewModel
{
    [Required(ErrorMessage = "Product name is required.")]
    [MaxLength(200, ErrorMessage = "Product name cannot exceed 200 characters.")]
    [Display(Name = "Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Product description is required.")]
    [MaxLength(1000, ErrorMessage = "Product description cannot exceed 1000 characters.")]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Product price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    [Display(Name = "Price (USD)")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Units in stock is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Units in stock must be zero or greater.")]
    [Display(Name = "Units in Stock")]
    public int UnitsInStock { get; set; }
}

public class EditProductViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Product name is required.")]
    [MaxLength(200, ErrorMessage = "Product name cannot exceed 200 characters.")]
    [Display(Name = "Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Product description is required.")]
    [MaxLength(1000, ErrorMessage = "Product description cannot exceed 1000 characters.")]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Product price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    [Display(Name = "Price (USD)")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Units in stock is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Units in stock must be zero or greater.")]
    [Display(Name = "Units in Stock")]
    public int UnitsInStock { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;
}

public class ProductExchangeRateViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal OriginalPrice { get; set; }
    public string OriginalCurrency { get; set; } = "USD";
    public decimal ConvertedPrice { get; set; }
    public string TargetCurrency { get; set; } = string.Empty;
    public decimal ExchangeRate { get; set; }
    public int UnitsInStock { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class ErrorViewModel
{
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    public string? Message { get; set; }
}

public class ProductListViewModel
{
    public IEnumerable<ProductExchangeRateViewModel> Products { get; set; } = Enumerable.Empty<ProductExchangeRateViewModel>();
    public string SelectedCurrency { get; set; } = "COP";
    public List<string> SupportedCurrencies { get; set; } = new();
}
