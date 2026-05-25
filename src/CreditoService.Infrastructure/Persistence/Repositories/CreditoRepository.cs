using CreditoService.Application.Abstractions.Persistence;
using CreditoService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CreditoService.Infrastructure.Persistence.Repositories;

public sealed class CreditoRepository : ICreditoRepository
{
    private readonly CreditoDbContext _dbContext;

    public CreditoRepository(CreditoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Credito credito, CancellationToken cancellationToken)
    {
        await _dbContext.Creditos.AddAsync(credito, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> ExistsByNumeroCreditoAsync(string numeroCredito, CancellationToken cancellationToken)
    {
        return _dbContext.Creditos
            .AsNoTracking()
            .AnyAsync(credito => credito.NumeroCredito == numeroCredito, cancellationToken);
    }

    public Task<Credito?> GetByNumeroCreditoAsync(string numeroCredito, CancellationToken cancellationToken)
    {
        return _dbContext.Creditos
            .AsNoTracking()
            .FirstOrDefaultAsync(credito => credito.NumeroCredito == numeroCredito, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Credito>> GetByNumeroNfseAsync(
        string numeroNfse,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Creditos
            .AsNoTracking()
            .Where(credito => credito.NumeroNfse == numeroNfse)
            .OrderBy(credito => credito.DataConstituicao)
            .ThenBy(credito => credito.NumeroCredito)
            .ToArrayAsync(cancellationToken);
    }
}
