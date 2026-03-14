namespace SCISalesTest.Application.DTOs.ExternalServices;

public class CurrencyConversionDto
{
    public decimal Amount { get; set; }
    public string SourceCurrency { get; set; } = string.Empty;
    public string TargetCurrency { get; set; } = string.Empty;
    public decimal ExchangeRate { get; set; }
    public decimal ConvertedAmount { get; set; }
}
