using System.Linq.Expressions;
using Swoms.Domain.Common;

namespace Swoms.Application.Common.Interfaces;

public interface IRepository<TEntity>
    where TEntity : BaseEntity
{
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default,
        params string[] includes);

    Task<IReadOnlyList<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default,
        params string[] includes);

    void Add(TEntity entity);

    void Update(TEntity entity);

    void Remove(TEntity entity);
}
