using SCISalesTest.Domain.Entities;

namespace SCISalesTest.Application.UnitTest.Builders;

public class ProductBuilder
{
    private int _id = 1;
    private string _name = "Test Product";
    private string _description = "Test Description";
    private decimal _price = 49.99m;
    private int _unitsInStock = 100;
    private bool _isActive = true;
    private DateTime _createdDate = DateTime.UtcNow;
    private DateTime? _modifiedDate;

    public ProductBuilder WithId(int id) { _id = id; return this; }
    public ProductBuilder WithName(string name) { _name = name; return this; }
    public ProductBuilder WithDescription(string description) { _description = description; return this; }
    public ProductBuilder WithPrice(decimal price) { _price = price; return this; }
    public ProductBuilder WithUnitsInStock(int units) { _unitsInStock = units; return this; }
    public ProductBuilder WithIsActive(bool isActive) { _isActive = isActive; return this; }
    public ProductBuilder WithCreatedDate(DateTime date) { _createdDate = date; return this; }
    public ProductBuilder WithModifiedDate(DateTime? date) { _modifiedDate = date; return this; }

    public Product Build() => new()
    {
        Id = _id,
        Name = _name,
        Description = _description,
        Price = _price,
        UnitsInStock = _unitsInStock,
        IsActive = _isActive,
        CreatedDate = _createdDate,
        ModifiedDate = _modifiedDate
    };
}
