using SCISalesTest.Application.DTOs.Products;

namespace SCISalesTest.Application.UnitTest.Builders;

public class CreateProductDtoBuilder
{
    private string _name = "Test Product";
    private string _description = "Test Description";
    private decimal _price = 49.99m;
    private int _unitsInStock = 100;

    public CreateProductDtoBuilder WithName(string name) { _name = name; return this; }
    public CreateProductDtoBuilder WithDescription(string description) { _description = description; return this; }
    public CreateProductDtoBuilder WithPrice(decimal price) { _price = price; return this; }
    public CreateProductDtoBuilder WithUnitsInStock(int units) { _unitsInStock = units; return this; }

    public CreateProductDto Build() => new()
    {
        Name = _name,
        Description = _description,
        Price = _price,
        UnitsInStock = _unitsInStock
    };
}
