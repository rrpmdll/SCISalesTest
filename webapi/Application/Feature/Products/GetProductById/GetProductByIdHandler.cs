using MediatR;
using SCISalesTest.Application.ApplicationServices;
using SCISalesTest.Application.DTOs.Products;

namespace SCISalesTest.Application.Feature.Products.GetProductById;

public class GetProductByIdHandler : 
    IRequestHandler<GetProductByIdQuery, ProductDto>
{
    private readonly ProductService _productService;

    public GetProductByIdHandler(
        ProductService productService
    )
    {
        _productService = productService;
    }

    public async Task<ProductDto> Handle(
        GetProductByIdQuery request, 
        CancellationToken cancellationToken
    ) => await _productService.GetProductByIdAsync(request.Id);
}
