using System.ComponentModel.DataAnnotations;

namespace SCISalesTest.WebApp.Models;

public class CurrencyConverterViewModel
{
    [Required(ErrorMessage = "Amount is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    [Display(Name = "Amount")]
    public decimal Amount { get; set; } = 1;

    [Required(ErrorMessage = "Source currency is required.")]
    [Display(Name = "From")]
    public string SourceCurrency { get; set; } = "USD";

    [Required(ErrorMessage = "Target currency is required.")]
    [Display(Name = "To")]
    public string TargetCurrency { get; set; } = "COP";

    public decimal? ConvertedAmount { get; set; }
    public decimal? ExchangeRate { get; set; }
    public bool HasResult => ConvertedAmount.HasValue;
    public List<string> SupportedCurrencies { get; set; } = new();
}
