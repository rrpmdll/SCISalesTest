using System.ComponentModel.DataAnnotations;
using MediatR;
using SCISalesTest.Application.DTOs.ExternalServices;

namespace SCISalesTest.Application.Feature.CurrencyConversion;

public record ConvertCurrencyQuery : IRequest<CurrencyConversionDto>
{
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public decimal Amount { get; init; }

    [Required(ErrorMessage = "Source currency code is required.")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency code must be exactly 3 characters.")]
    public string SourceCurrency { get; init; } = string.Empty;

    [Required(ErrorMessage = "Target currency code is required.")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency code must be exactly 3 characters.")]
    public string TargetCurrency { get; init; } = string.Empty;
}
