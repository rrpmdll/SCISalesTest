using SCISalesTest.Application.DTOs.Products;

namespace SCISalesTest.Application.UnitTest.Builders;

public class UpdateProductDtoBuilder
{
    private int _id = 1;
    private string _name = "Updated Product";
    private string _description = "Updated Description";
    private decimal _price = 59.99m;
    private int _unitsInStock = 50;
    private bool _isActive = true;

    public UpdateProductDtoBuilder WithId(int id) { _id = id; return this; }
    public UpdateProductDtoBuilder WithName(string name) { _name = name; return this; }
    public UpdateProductDtoBuilder WithDescription(string description) { _description = description; return this; }
    public UpdateProductDtoBuilder WithPrice(decimal price) { _price = price; return this; }
    public UpdateProductDtoBuilder WithUnitsInStock(int units) { _unitsInStock = units; return this; }
    public UpdateProductDtoBuilder WithIsActive(bool isActive) { _isActive = isActive; return this; }

    public UpdateProductDto Build() => new()
    {
        Id = _id,
        Name = _name,
        Description = _description,
        Price = _price,
        UnitsInStock = _unitsInStock,
        IsActive = _isActive
    };
}
