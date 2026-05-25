using CreditoService.Application.Abstractions.Persistence;
using CreditoService.Application.DTOs.CreditoConstituido;
using CreditoService.Application.Mapping;
using CreditoService.Domain.Entities;
using FluentValidation;

namespace CreditoService.Application.Services;

public sealed class CreditoMessageProcessor : ICreditoMessageProcessor
{
    private readonly ICreditoRepository _creditoRepository;
    private readonly IValidator<CreditoConstituidoInputModel> _validator;

    public CreditoMessageProcessor(
        ICreditoRepository creditoRepository,
        IValidator<CreditoConstituidoInputModel> validator)
    {
        _creditoRepository = creditoRepository;
        _validator = validator;
    }

    public async Task ProcessAsync(CreditoConstituidoInputModel inputModel, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(inputModel, cancellationToken);

        var exists = await _creditoRepository.ExistsByNumeroCreditoAsync(inputModel.NumeroCredito, cancellationToken);
        if (exists)
        {
            return;
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
