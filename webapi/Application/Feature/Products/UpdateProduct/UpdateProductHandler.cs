using AutoMapper;
using MediatR;
using SCISalesTest.Application.ApplicationServices;
using SCISalesTest.Application.DTOs.Products;

namespace SCISalesTest.Application.Feature.Products.UpdateProduct;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ProductDto>
{
    private readonly ProductService _productService;
    private readonly IMapper _mapper;

    public UpdateProductHandler(
        ProductService productService,
        IMapper mapper
    )
    {
        _productService = productService;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(
        UpdateProductCommand request, 
        CancellationToken cancellationToken
    ) => await _productService.UpdateProductAsync(
        _mapper.Map<UpdateProductDto>(request)
    );
}
