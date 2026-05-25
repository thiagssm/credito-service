using CreditoService.Domain.Entities;

namespace CreditoService.Application.Abstractions.Persistence;

public interface ICreditoRepository
{
    Task AddAsync(Credito credito, CancellationToken cancellationToken);
    Task<bool> ExistsByNumeroCreditoAsync(string numeroCredito, CancellationToken cancellationToken);
    Task<Credito?> GetByNumeroCreditoAsync(string numeroCredito, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Credito>> GetByNumeroNfseAsync(string numeroNfse, CancellationToken cancellationToken);
}
