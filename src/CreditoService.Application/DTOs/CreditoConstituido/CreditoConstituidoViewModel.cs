using CreditoService.Application.Mapping;
using CreditoService.Domain.Entities;

namespace CreditoService.Application.DTOs.CreditoConstituido;

public sealed record CreditoConstituidoViewModel(
    string NumeroCredito,
    string NumeroNfse,
    DateOnly DataConstituicao,
    decimal ValorIssqn,
    string TipoCredito,
    string SimplesNacional,
    decimal Aliquota,
    decimal ValorFaturado,
    decimal ValorDeducao,
    decimal BaseCalculo)
{
    public static CreditoConstituidoViewModel FromEntity(Credito credito)
    {
        return new CreditoConstituidoViewModel(
            credito.NumeroCredito,
            credito.NumeroNfse,
            credito.DataConstituicao,
            credito.ValorIssqn,
            credito.TipoCredito,
            SimplesNacionalParser.Format(credito.SimplesNacional),
            credito.Aliquota,
            credito.ValorFaturado,
            credito.ValorDeducao,
            credito.BaseCalculo);
    }
}
