using CreditoService.Application.Abstractions.Persistence;
using CreditoService.Application.DTOs.CreditoConstituido;
using CreditoService.Application.Services;
using CreditoService.Application.Validation;
using CreditoService.Domain.Entities;
using FluentAssertions;
using Moq;

namespace CreditoService.Tests.Application;

public sealed class CreditoMessageProcessorTests
{
    private readonly Mock<ICreditoRepository> _creditoRepository = new();

    [Fact]
    public async Task ProcessAsync_DevePersistirCreditoQuandoNaoExistir()
    {
        var inputModel = CriarInputModel("123456", "Sim");
        _creditoRepository
            .Setup(repository => repository.ExistsByNumeroCreditoAsync("123456", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var processor = CriarProcessor();

        await processor.ProcessAsync(inputModel, CancellationToken.None);

        _creditoRepository.Verify(
            repository => repository.AddAsync(
                It.Is<Credito>(credito =>
                    credito.NumeroCredito == "123456"
                    && credito.NumeroNfse == "7891011"
                    && credito.SimplesNacional),
                CancellationToken.None),
            Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_DeveIgnorarCreditoDuplicado()
    {
        var inputModel = CriarInputModel("123456", "Não");
        _creditoRepository
            .Setup(repository => repository.ExistsByNumeroCreditoAsync("123456", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var processor = CriarProcessor();

        await processor.ProcessAsync(inputModel, CancellationToken.None);

        _creditoRepository.Verify(
            repository => repository.AddAsync(It.IsAny<Credito>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task ProcessAsync_DeveValidarSimplesNacional()
    {
        var inputModel = CriarInputModel("123456", "Talvez");
        var processor = CriarProcessor();

        var act = () => processor.ProcessAsync(inputModel, CancellationToken.None);

        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    private CreditoMessageProcessor CriarProcessor()
    {
        return new CreditoMessageProcessor(
            _creditoRepository.Object,
            new CreditoConstituidoInputModelValidator());
    }

    private static CreditoConstituidoInputModel CriarInputModel(string numeroCredito, string simplesNacional)
    {
        return new CreditoConstituidoInputModel(
            numeroCredito,
            "7891011",
            new DateOnly(2024, 2, 25),
            1500.75m,
            "ISSQN",
            simplesNacional,
            5.0m,
            30000.00m,
            5000.00m,
            25000.00m);
    }
}
