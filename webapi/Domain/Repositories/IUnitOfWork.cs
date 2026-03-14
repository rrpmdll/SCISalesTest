using System.Data;

namespace SCISalesTest.Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    IDbTransaction? Transaction { get; }
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
