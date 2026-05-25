using CreditoService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CreditoService.Infrastructure.Persistence;

public sealed class CreditoDbContext : DbContext
{
    public CreditoDbContext(DbContextOptions<CreditoDbContext> options)
        : base(options)
    {
    }

    public DbSet<Credito> Creditos => Set<Credito>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CreditoDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
