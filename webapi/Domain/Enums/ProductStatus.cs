using System.ComponentModel;

namespace SCISalesTest.Domain.Enums;

public enum ProductStatus
{
    [Description("Active")]
    Active = 1,

    [Description("Inactive")]
    Inactive = 0
}
