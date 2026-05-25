using CreditoService.Application.Abstractions.Messaging;
using CreditoService.Application.Abstractions.Persistence;
using CreditoService.Application.DTOs.CreditoConstituido;
using FluentValidation;

namespace CreditoService.Application.Services;

public sealed class CreditoApplicationService : ICreditoApplicationService
{
    private readonly ICreditoRepository _creditoRepository;
    private readonly ICreditoMessageProducer _creditoMessageProducer;
    private readonly IValidator<CreditoConstituidoInputModel> _validator;

    public CreditoApplicationService(
        ICreditoRepository creditoRepository,
        ICreditoMessageProducer creditoMessageProducer,
        IValidator<CreditoConstituidoInputModel> validator)
    {
        _creditoRepository = creditoRepository;
        _creditoMessageProducer = creditoMessageProducer;
        _validator = validator;
    }

    public async Task IntegrarCreditoConstituidoAsync(
        IReadOnlyCollection<CreditoConstituidoInputModel> creditos,
        CancellationToken cancellationToken)
    {
        if (creditos.Count == 0)
        {
            throw new ValidationException("Informe ao menos um credito constituido.");
        }

        foreach (var inputModel in creditos)
        {
            await _validator.ValidateAndThrowAsync(inputModel, cancellationToken);
            await _creditoMessageProducer.PublishAsync(inputModel, cancellationToken);
        }
    }

    public async Task<CreditoConstituidoViewModel?> ObterPorNumeroCreditoAsync(
        string numeroCredito,
        CancellationToken cancellationToken)
    {
        var credito = await _creditoRepository.GetByNumeroCreditoAsync(numeroCredito, cancellationToken);
        return credito is null ? null : CreditoConstituidoViewModel.FromEntity(credito);
    }

    public async Task<IReadOnlyCollection<CreditoConstituidoViewModel>> ObterPorNumeroNfseAsync(
        string numeroNfse,
        CancellationToken cancellationToken)
    {
        var creditos = await _creditoRepository.GetByNumeroNfseAsync(numeroNfse, cancellationToken);
        return creditos.Select(CreditoConstituidoViewModel.FromEntity).ToArray();
    }
}
