using System.Data;
using SCISalesTest.Domain.Repositories;
using SCISalesTest.Infrastructure.Context;

namespace SCISalesTest.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly DapperContext _context;
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;
    private bool _disposed;

    public UnitOfWork(DapperContext context)
    {
        _context = context;
    }

    public IDbTransaction? Transaction => _transaction;

    public async Task BeginTransactionAsync()
    {
        _connection = _context.CreateConnection();
        _connection.Open();
        _transaction = _connection.BeginTransaction();
        await Task.CompletedTask;
    }

    public async Task CommitAsync()
    {
        _transaction?.Commit();
        await Task.CompletedTask;
    }

    public async Task RollbackAsync()
    {
        _transaction?.Rollback();
        await Task.CompletedTask;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _connection?.Dispose();
            }
            _disposed = true;
        }
    }
}
