using MediatR;
using SCISalesTest.Application.ApplicationServices;

namespace SCISalesTest.Application.Feature.Products.DeleteProduct;

public class DeleteProductHandler : 
    IRequestHandler<DeleteProductCommand, bool>
{
    private readonly ProductService _productService;

    public DeleteProductHandler(
        ProductService productService
    )
    {
        _productService = productService;
    }

    public async Task<bool> Handle(
        DeleteProductCommand request, 
        CancellationToken cancellationToken
    ) => await _productService.DeleteProductAsync(request.Id);
}
