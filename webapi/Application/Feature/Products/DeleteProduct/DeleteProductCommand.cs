using System.ComponentModel.DataAnnotations;
using MediatR;

namespace SCISalesTest.Application.Feature.Products.DeleteProduct;

public record DeleteProductCommand : IRequest<bool>
{
    [Required]
    public int Id { get; init; }
}
