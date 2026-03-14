using MediatR;
using SCISalesTest.Application.ApplicationServices;
using SCISalesTest.Application.DTOs.Products;

namespace SCISalesTest.Application.Feature.Products.GetProductWithExchangeRate;

public class GetProductWithExchangeRateHandler : 
    IRequestHandler<GetProductWithExchangeRateQuery, ProductWithExchangeRateDto>
{
    private readonly ProductService _productService;

    public GetProductWithExchangeRateHandler(
        ProductService productService
    )
    {
        _productService = productService;
    }

    public async Task<ProductWithExchangeRateDto> Handle(
        GetProductWithExchangeRateQuery request, 
        CancellationToken cancellationToken
    ) => await _productService.GetProductWithExchangeRateAsync(
        request.ProductId, 
        request.TargetCurrency
    );
}
