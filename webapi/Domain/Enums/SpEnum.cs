using System.ComponentModel;

namespace SCISalesTest.Domain.Enums;

public enum SpEnum
{
    [Description("SpCreateProduct")]
    SpCreateProduct,

    [Description("SpGetAllProducts")]
    SpGetAllProducts,

    [Description("SpGetProductById")]
    SpGetProductById,

    [Description("SpUpdateProduct")]
    SpUpdateProduct,

    [Description("SpDeleteProduct")]
    SpDeleteProduct
}
