namespace SCISalesTest.Application.DTOs.Products;

public class UpdateProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int UnitsInStock { get; set; }
    public bool IsActive { get; set; }
}
