namespace SCISalesTest.Application.DTOs.Products;

public class ProductWithExchangeRateDto
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
