using System.ComponentModel.DataAnnotations;
using MediatR;
using SCISalesTest.Application.DTOs.Products;

namespace SCISalesTest.Application.Feature.Products.GetProductWithExchangeRate;

public record GetProductWithExchangeRateQuery : IRequest<ProductWithExchangeRateDto>
{
    [Required]
    public int ProductId { get; init; }

    [Required(ErrorMessage = "Target currency code is required.")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency code must be exactly 3 characters (e.g., COP, EUR, GBP).")]
    public string TargetCurrency { get; init; } = string.Empty;
}
