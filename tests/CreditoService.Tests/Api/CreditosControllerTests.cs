using CreditoService.Api.Controllers;
using CreditoService.Application.DTOs.Common;
using CreditoService.Application.DTOs.CreditoConstituido;
using CreditoService.Application.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CreditoService.Tests.Api;

public sealed class CreditosControllerTests
{
    private readonly Mock<ICreditoApplicationService> _creditoApplicationService = new();

    [Fact]
    public async Task IntegrarCreditoConstituidoAsync_DeveRetornarAccepted()
    {
        var inputModels = new[]
        {
            CriarInputModel("123456")
        };

        var controller = new CreditosController(_creditoApplicationService.Object);

        var result = await controller.IntegrarCreditoConstituidoAsync(inputModels, CancellationToken.None);

        var acceptedResult = result.Should().BeOfType<AcceptedResult>().Subject;
        acceptedResult.Value.Should().BeEquivalentTo(new SuccessViewModel(true));
        _creditoApplicationService.Verify(
            service => service.IntegrarCreditoConstituidoAsync(inputModels, CancellationToken.None),
            Times.Once);
    }

    [Fact]
    public async Task ObterPorNumeroCreditoAsync_DeveRetornarNotFoundQuandoNaoExistir()
    {
        _creditoApplicationService
            .Setup(service => service.ObterPorNumeroCreditoAsync("999999", It.IsAny<CancellationToken>()))
            .ReturnsAsync((CreditoConstituidoViewModel?)null);

        var controller = new CreditosController(_creditoApplicationService.Object);

        var result = await controller.ObterPorNumeroCreditoAsync("999999", CancellationToken.None);

        result.Should().BeOfType<NotFoundResult>();
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
