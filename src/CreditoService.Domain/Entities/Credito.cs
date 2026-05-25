namespace CreditoService.Domain.Entities;

public sealed class Credito
{
    private Credito()
    {
    }

    private Credito(
        string numeroCredito,
        string numeroNfse,
        DateOnly dataConstituicao,
        decimal valorIssqn,
        string tipoCredito,
        bool simplesNacional,
        decimal aliquota,
        decimal valorFaturado,
        decimal valorDeducao,
        decimal baseCalculo)
    {
        NumeroCredito = numeroCredito;
        NumeroNfse = numeroNfse;
        DataConstituicao = dataConstituicao;
        ValorIssqn = valorIssqn;
        TipoCredito = tipoCredito;
        SimplesNacional = simplesNacional;
        Aliquota = aliquota;
        ValorFaturado = valorFaturado;
        ValorDeducao = valorDeducao;
        BaseCalculo = baseCalculo;
    }

    public long Id { get; private set; }
    public string NumeroCredito { get; private set; } = string.Empty;
    public string NumeroNfse { get; private set; } = string.Empty;
    public DateOnly DataConstituicao { get; private set; }
    public decimal ValorIssqn { get; private set; }
    public string TipoCredito { get; private set; } = string.Empty;
    public bool SimplesNacional { get; private set; }
    public decimal Aliquota { get; private set; }
    public decimal ValorFaturado { get; private set; }
    public decimal ValorDeducao { get; private set; }
    public decimal BaseCalculo { get; private set; }

    public static Credito Criar(
        string numeroCredito,
        string numeroNfse,
        DateOnly dataConstituicao,
        decimal valorIssqn,
        string tipoCredito,
        bool simplesNacional,
        decimal aliquota,
        decimal valorFaturado,
        decimal valorDeducao,
        decimal baseCalculo)
    {
        return new Credito(
            numeroCredito.Trim(),
            numeroNfse.Trim(),
            dataConstituicao,
            valorIssqn,
            tipoCredito.Trim(),
            simplesNacional,
            aliquota,
            valorFaturado,
            valorDeducao,
            baseCalculo);
    }
}
