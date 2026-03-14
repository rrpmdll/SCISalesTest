using SCISalesTest.Domain.Entities;
using SCISalesTest.Domain.Enums;
using SCISalesTest.Domain.Repositories;
using SCISalesTest.Infrastructure.Context;
using SCISalesTest.Infrastructure.Helpers;

namespace SCISalesTest.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly DapperContext _context;

    public ProductRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();
        return await StoredProcedureHelper.ExecuteStoredProcedureSingleAsync<Product>(
            connection,
            SqlProviderHelper.GetStoredProcedureName(SpEnum.SpGetProductById),
            new { Id = id }
        );
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();
        return await StoredProcedureHelper.ExecuteStoredProcedureListAsync<Product>(
            connection,
            SqlProviderHelper.GetStoredProcedureName(SpEnum.SpGetAllProducts)
        );
    }

    public async Task<Product> CreateAsync(Product product)
    {
        using var connection = _context.CreateConnection();
        var createdProduct = await StoredProcedureHelper.ExecuteStoredProcedureSingleAsync<Product>(
            connection,
            SqlProviderHelper.GetStoredProcedureName(SpEnum.SpCreateProduct),
            new
            {
                product.Name,
                product.Description,
                product.Price,
                product.UnitsInStock
            }
        );

        return createdProduct!;
    }

    public async Task<Product?> UpdateAsync(Product product)
    {
        using var connection = _context.CreateConnection();
        return await StoredProcedureHelper.ExecuteStoredProcedureSingleAsync<Product>(
            connection,
            SqlProviderHelper.GetStoredProcedureName(SpEnum.SpUpdateProduct),
            new
            {
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.UnitsInStock,
                product.IsActive
            }
        );
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _context.CreateConnection();
        var rowsAffected = await StoredProcedureHelper.ExecuteStoredProcedureScalarAsync<int>(
            connection,
            SqlProviderHelper.GetStoredProcedureName(SpEnum.SpDeleteProduct),
            new { Id = id }
        );

        return rowsAffected > 0;
    }
}
