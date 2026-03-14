using MediatR;
using SCISalesTest.Application.DTOs.Products;

namespace SCISalesTest.Application.Feature.Products.GetProductsWithExchangeRate;

public record GetProductsWithExchangeRateQuery : 
    IRequest<IEnumerable<ProductWithExchangeRateDto>>
{
    public string TargetCurrency { get; init; } = string.Empty;
}
