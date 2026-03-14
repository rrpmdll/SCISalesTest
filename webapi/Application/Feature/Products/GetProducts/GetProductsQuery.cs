using MediatR;
using SCISalesTest.Application.DTOs.Products;

namespace SCISalesTest.Application.Feature.Products.GetProducts;

public record GetProductsQuery : IRequest<IEnumerable<ProductDto>>;
