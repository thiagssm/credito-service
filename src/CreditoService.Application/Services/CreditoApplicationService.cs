using CreditoService.Application.Abstractions.Persistence;
using CreditoService.Application.DTOs.CreditoConstituido;
using CreditoService.Application.Mapping;
using CreditoService.Domain.Entities;
using FluentValidation;

namespace CreditoService.Application.Services;

public sealed class CreditoApplicationService : ICreditoApplicationService
{
    private readonly ICreditoRepository _creditoRepository;
    private readonly IValidator<CreditoConstituidoInputModel> _validator;

    public CreditoApplicationService(
        ICreditoRepository creditoRepository,
        IValidator<CreditoConstituidoInputModel> validator)
    {
        _creditoRepository = creditoRepository;
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

            var exists = await _creditoRepository.ExistsByNumeroCreditoAsync(
                inputModel.NumeroCredito,
                cancellationToken);

            if (exists)
            {
                continue;
            }

            var credito = Credito.Criar(
                inputModel.NumeroCredito,
                inputModel.NumeroNfse,
                inputModel.DataConstituicao,
                inputModel.ValorIssqn,
                inputModel.TipoCredito,
                SimplesNacionalParser.Parse(inputModel.SimplesNacional),
                inputModel.Aliquota,
                inputModel.ValorFaturado,
                inputModel.ValorDeducao,
                inputModel.BaseCalculo);

            await _creditoRepository.AddAsync(credito, cancellationToken);
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
