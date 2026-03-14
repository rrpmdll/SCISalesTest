using AutoMapper;
using MediatR;
using SCISalesTest.Application.ApplicationServices;
using SCISalesTest.Application.DTOs.Products;

namespace SCISalesTest.Application.Feature.Products.CreateProduct;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly ProductService _productService;
    private readonly IMapper _mapper;

    public CreateProductHandler(ProductService productService, IMapper mapper)
    {
        _productService = productService;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(
        CreateProductCommand request, 
        CancellationToken cancellationToken
    ) => await _productService.CreateProductAsync(
        _mapper.Map<CreateProductDto>(request)
    );
}
