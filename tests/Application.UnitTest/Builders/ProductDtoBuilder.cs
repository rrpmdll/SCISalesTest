using SCISalesTest.Application.DTOs.Products;

namespace SCISalesTest.Application.UnitTest.Builders;

public class ProductDtoBuilder
{
    private int _id = 1;
    private string _name = "Test Product";
    private string _description = "Test Description";
    private decimal _price = 49.99m;
    private int _unitsInStock = 100;
    private bool _isActive = true;
    private DateTime _createdDate = DateTime.UtcNow;
    private DateTime? _modifiedDate;

    public ProductDtoBuilder WithId(int id) { _id = id; return this; }
    public ProductDtoBuilder WithName(string name) { _name = name; return this; }
    public ProductDtoBuilder WithDescription(string description) { _description = description; return this; }
    public ProductDtoBuilder WithPrice(decimal price) { _price = price; return this; }
    public ProductDtoBuilder WithUnitsInStock(int units) { _unitsInStock = units; return this; }
    public ProductDtoBuilder WithIsActive(bool isActive) { _isActive = isActive; return this; }
    public ProductDtoBuilder WithCreatedDate(DateTime date) { _createdDate = date; return this; }
    public ProductDtoBuilder WithModifiedDate(DateTime? date) { _modifiedDate = date; return this; }

    public ProductDto Build() => new()
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
