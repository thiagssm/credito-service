using CreditoService.Application.Abstractions.Messaging;
using CreditoService.Application.Abstractions.Persistence;
using CreditoService.Application.DTOs.CreditoConstituido;
using CreditoService.Application.Services;
using CreditoService.Application.Validation;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace CreditoService.Tests.Application;

public sealed class CreditoApplicationServiceTests
{
    private readonly Mock<ICreditoRepository> _creditoRepository = new();
    private readonly Mock<ICreditoMessageProducer> _creditoMessageProducer = new();

    [Fact]
    public async Task IntegrarCreditoConstituidoAsync_DevePublicarUmaMensagemPorCredito()
    {
        var inputModels = new[]
        {
            CriarInputModel("123456"),
            CriarInputModel("789012")
        };

        var service = CriarService();

        await service.IntegrarCreditoConstituidoAsync(inputModels, CancellationToken.None);

        _creditoMessageProducer.Verify(
            producer => producer.PublishAsync(It.IsAny<CreditoConstituidoInputModel>(), CancellationToken.None),
            Times.Exactly(2));
    }

    [Fact]
    public async Task IntegrarCreditoConstituidoAsync_DeveRejeitarListaVazia()
    {
        var service = CriarService();

        var act = () => service.IntegrarCreditoConstituidoAsync(
            Array.Empty<CreditoConstituidoInputModel>(),
            CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>();
    }

    private CreditoApplicationService CriarService()
    {
        return new CreditoApplicationService(
            _creditoRepository.Object,
            _creditoMessageProducer.Object,
            new CreditoConstituidoInputModelValidator());
    }

    private static CreditoConstituidoInputModel CriarInputModel(string numeroCredito)
    {
        return new CreditoConstituidoInputModel(
            numeroCredito,
            "7891011",
            new DateOnly(2024, 2, 25),
            1500.75m,
            "ISSQN",
            "Sim",
            5.0m,
            30000.00m,
            5000.00m,
            25000.00m);
    }
}
