using System.Data;
using Dapper;

namespace SCISalesTest.Infrastructure.Helpers;

public static class StoredProcedureHelper
{
    public static async Task<T?> ExecuteStoredProcedureSingleAsync<T>(
        IDbConnection connection,
        string storedProcedureName,
        object? parameters = null,
        IDbTransaction? transaction = null)
    {
        return await connection.QueryFirstOrDefaultAsync<T>(
            storedProcedureName,
            parameters,
            transaction,
            commandType: CommandType.StoredProcedure);
    }

    public static async Task<IEnumerable<T>> ExecuteStoredProcedureListAsync<T>(
        IDbConnection connection,
        string storedProcedureName,
        object? parameters = null,
        IDbTransaction? transaction = null)
    {
        return await connection.QueryAsync<T>(
            storedProcedureName,
            parameters,
            transaction,
            commandType: CommandType.StoredProcedure);
    }

    public static async Task<int> ExecuteStoredProcedureAsync(
        IDbConnection connection,
        string storedProcedureName,
        object? parameters = null,
        IDbTransaction? transaction = null)
    {
        return await connection.ExecuteAsync(
            storedProcedureName,
            parameters,
            transaction,
            commandType: CommandType.StoredProcedure);
    }

    public static async Task<T?> ExecuteStoredProcedureScalarAsync<T>(
        IDbConnection connection,
        string storedProcedureName,
        object? parameters = null,
        IDbTransaction? transaction = null)
    {
        return await connection.ExecuteScalarAsync<T>(
            storedProcedureName,
            parameters,
            transaction,
            commandType: CommandType.StoredProcedure);
    }
}
