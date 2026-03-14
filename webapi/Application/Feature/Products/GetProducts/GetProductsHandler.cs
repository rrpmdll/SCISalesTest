using MediatR;
using SCISalesTest.Application.ApplicationServices;
using SCISalesTest.Application.DTOs.Products;

namespace SCISalesTest.Application.Feature.Products.GetProducts;

public class GetProductsHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
{
    private readonly ProductService _productService;

    public GetProductsHandler(ProductService productService)
    {
        _productService = productService;
    }

    public async Task<IEnumerable<ProductDto>> Handle(
        GetProductsQuery request, 
        CancellationToken cancellationToken
    ) => await _productService.GetAllProductsAsync();
}
