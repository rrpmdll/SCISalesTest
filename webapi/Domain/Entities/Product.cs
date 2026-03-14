using SCISalesTest.Domain.Entities.Base;

namespace SCISalesTest.Domain.Entities;

public class Product : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int UnitsInStock { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }

    public void Update(
        string name, 
        string description, 
        decimal price, 
        int unitsInStock, 
        bool isActive
    )
    {
        Name = name;
        Description = description;
        Price = price;
        UnitsInStock = unitsInStock;
        IsActive = isActive;
        ModifiedDate = DateTime.UtcNow;
    }
}
