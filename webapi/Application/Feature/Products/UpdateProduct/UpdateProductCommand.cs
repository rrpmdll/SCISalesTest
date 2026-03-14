using System.ComponentModel.DataAnnotations;
using MediatR;
using SCISalesTest.Application.DTOs.Products;

namespace SCISalesTest.Application.Feature.Products.UpdateProduct;

public record UpdateProductCommand : IRequest<ProductDto>
{
    [Required]
    public int Id { get; init; }

    [Required(ErrorMessage = "Product name is required.")]
    [MaxLength(200, ErrorMessage = "Product name cannot exceed 200 characters.")]
    public string Name { get; init; } = string.Empty;

    [Required(ErrorMessage = "Product description is required.")]
    [MaxLength(1000, ErrorMessage = "Product description cannot exceed 1000 characters.")]
    public string Description { get; init; } = string.Empty;

    [Required(ErrorMessage = "Product price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Product price must be greater than zero.")]
    public decimal Price { get; init; }

    [Required(ErrorMessage = "Units in stock is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Units in stock must be zero or greater.")]
    public int UnitsInStock { get; init; }

    public bool IsActive { get; init; } = true;
}
