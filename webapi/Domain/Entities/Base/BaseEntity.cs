namespace SCISalesTest.Domain.Entities.Base;

public abstract class BaseEntity<TId> where TId : notnull
{
    public TId Id { get; set; } = default!;
}
