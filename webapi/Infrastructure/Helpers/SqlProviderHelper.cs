using System.Resources;
using SCISalesTest.Domain.Enums;
using SCISalesTest.Domain.Extensions;

namespace SCISalesTest.Infrastructure.Helpers;

public static class SqlProviderHelper
{
    private static readonly ResourceManager _resourceManager =
        new("SCISalesTest.Infrastructure.Resources.SqlProvider", typeof(SqlProviderHelper).Assembly);

    public static string GetStoredProcedureName(SpEnum storedProcedure)
    {
        var key = storedProcedure.GetDescription();
        return _resourceManager.GetString(key)
            ?? throw new InvalidOperationException($"Stored procedure name not found for key '{key}' in SqlProvider resource.");
    }
}
