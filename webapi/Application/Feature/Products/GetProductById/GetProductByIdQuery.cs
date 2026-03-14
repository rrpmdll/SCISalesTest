using System.ComponentModel.DataAnnotations;
using MediatR;
using SCISalesTest.Application.DTOs.Products;

namespace SCISalesTest.Application.Feature.Products.GetProductById;

public record GetProductByIdQuery : IRequest<ProductDto>
{
    [Required]
    public int Id { get; init; }
}
