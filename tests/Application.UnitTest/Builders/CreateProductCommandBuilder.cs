using SCISalesTest.Application.Feature.Products.CreateProduct;

namespace SCISalesTest.Application.UnitTest.Builders;

public class CreateProductCommandBuilder
{
    private string _name = "Test Product";
    private string _description = "Test Description";
    private decimal _price = 49.99m;
    private int _unitsInStock = 100;

    public CreateProductCommandBuilder WithName(string name) { _name = name; return this; }
    public CreateProductCommandBuilder WithDescription(string description) { _description = description; return this; }
    public CreateProductCommandBuilder WithPrice(decimal price) { _price = price; return this; }
    public CreateProductCommandBuilder WithUnitsInStock(int units) { _unitsInStock = units; return this; }

    public CreateProductCommand Build() => new()
    {
        Name = _name,
        Description = _description,
        Price = _price,
        UnitsInStock = _unitsInStock
    };
}
