using CreditoService.Application.DTOs.CreditoConstituido;

namespace CreditoService.Application.Services;

public interface ICreditoApplicationService
{
    Task IntegrarCreditoConstituidoAsync(
        IReadOnlyCollection<CreditoConstituidoInputModel> creditos,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<CreditoConstituidoViewModel>> ObterPorNumeroNfseAsync(
        string numeroNfse,
        CancellationToken cancellationToken);

    Task<CreditoConstituidoViewModel?> ObterPorNumeroCreditoAsync(
        string numeroCredito,
        CancellationToken cancellationToken);
}
