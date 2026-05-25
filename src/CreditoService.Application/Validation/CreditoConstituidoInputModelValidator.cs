using CreditoService.Application.DTOs.CreditoConstituido;
using CreditoService.Application.Mapping;
using FluentValidation;

namespace CreditoService.Application.Validation;

public sealed class CreditoConstituidoInputModelValidator : AbstractValidator<CreditoConstituidoInputModel>
{
    public CreditoConstituidoInputModelValidator()
    {
        RuleFor(credito => credito.NumeroCredito).NotEmpty().MaximumLength(50);
        RuleFor(credito => credito.NumeroNfse).NotEmpty().MaximumLength(50);
        RuleFor(credito => credito.DataConstituicao).NotEqual(default(DateOnly));
        RuleFor(credito => credito.ValorIssqn).GreaterThan(0);
        RuleFor(credito => credito.TipoCredito).NotEmpty().MaximumLength(50);
        RuleFor(credito => credito.SimplesNacional)
            .NotEmpty()
            .Must(SimplesNacionalParser.IsValid)
            .WithMessage("SimplesNacional deve ser informado como Sim ou Nao.");
        RuleFor(credito => credito.Aliquota).GreaterThan(0);
        RuleFor(credito => credito.ValorFaturado).GreaterThanOrEqualTo(0);
        RuleFor(credito => credito.ValorDeducao).GreaterThanOrEqualTo(0);
        RuleFor(credito => credito.BaseCalculo).GreaterThanOrEqualTo(0);
    }
}
