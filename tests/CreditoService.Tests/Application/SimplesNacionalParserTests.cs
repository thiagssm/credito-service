using CreditoService.Application.Mapping;
using FluentAssertions;

namespace CreditoService.Tests.Application;

public sealed class SimplesNacionalParserTests
{
    [Theory]
    [InlineData("Sim", true)]
    [InlineData("S", true)]
    [InlineData("Não", false)]
    [InlineData("Nao", false)]
    [InlineData("N", false)]
    public void Parse_DeveConverterValoresAceitos(string value, bool expected)
    {
        var result = SimplesNacionalParser.Parse(value);

        result.Should().Be(expected);
    }

    [Fact]
    public void Format_DeveRetornarContratoDaApi()
    {
        SimplesNacionalParser.Format(true).Should().Be("Sim");
        SimplesNacionalParser.Format(false).Should().Be("Não");
    }
}
