using Swoms.Application.Common.Interfaces;
using Swoms.Domain.Common;
using Swoms.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Swoms.Infrastructure.Persistence;

public sealed class SwomsDbContext : DbContext, IUnitOfWork
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public SwomsDbContext(
        DbContextOptions<SwomsDbContext> options,
        ICurrentUserService currentUserService,
        IDateTimeProvider dateTimeProvider)
        : base(options)
    {
        _currentUserService = currentUserService;
        _dateTimeProvider = dateTimeProvider;
    }

    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public DbSet<Product> Products => Set<Product>();

    public DbSet<Warehouse> Warehouses => Set<Warehouse>();

    public DbSet<StockItem> StockItems => Set<StockItem>();

    public DbSet<StockMovement> StockMovements => Set<StockMovement>();

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SwomsDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAtUtc = _dateTimeProvider.UtcNow;
                entry.Entity.CreatedBy = _currentUserService.Email;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedAtUtc = _dateTimeProvider.UtcNow;
                entry.Entity.ModifiedBy = _currentUserService.Email;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
