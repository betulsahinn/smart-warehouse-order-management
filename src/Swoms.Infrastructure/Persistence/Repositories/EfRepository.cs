using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Swoms.Application.Common.Interfaces;
using Swoms.Domain.Common;

namespace Swoms.Infrastructure.Persistence.Repositories;

public sealed class EfRepository<TEntity> : IRepository<TEntity>
    where TEntity : BaseEntity
{
    private readonly SwomsDbContext _dbContext;

    public EfRepository(SwomsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<TEntity>().FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }

    public Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default,
        params string[] includes)
    {
        return ApplyIncludes(_dbContext.Set<TEntity>().AsQueryable(), includes)
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default,
        params string[] includes)
    {
        var query = ApplyIncludes(_dbContext.Set<TEntity>().AsQueryable(), includes);

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public void Add(TEntity entity) => _dbContext.Set<TEntity>().Add(entity);

    public void Update(TEntity entity) => _dbContext.Set<TEntity>().Update(entity);

    public void Remove(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);

    private static IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query, IEnumerable<string> includes)
    {
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return query;
    }
}
