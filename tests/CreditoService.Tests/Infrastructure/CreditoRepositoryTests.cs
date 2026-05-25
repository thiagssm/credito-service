using CreditoService.Domain.Entities;
using CreditoService.Infrastructure.Persistence;
using CreditoService.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CreditoService.Tests.Infrastructure;

public sealed class CreditoRepositoryTests
{
    [Fact]
    public async Task GetByNumeroNfseAsync_DeveRetornarCreditosDaNfseOrdenados()
    {
        await using var dbContext = CriarDbContext();
        var repository = new CreditoRepository(dbContext);

        await repository.AddAsync(CriarCredito("789012", "7891011", new DateOnly(2024, 2, 26)), CancellationToken.None);
        await repository.AddAsync(CriarCredito("123456", "7891011", new DateOnly(2024, 2, 25)), CancellationToken.None);
        await repository.AddAsync(CriarCredito("654321", "1122334", new DateOnly(2024, 1, 15)), CancellationToken.None);

        var result = await repository.GetByNumeroNfseAsync("7891011", CancellationToken.None);

        result.Should().HaveCount(2);
        result.Select(credito => credito.NumeroCredito).Should().ContainInOrder("123456", "789012");
    }

    [Fact]
    public async Task GetByNumeroCreditoAsync_DeveRetornarCreditoEspecifico()
    {
        await using var dbContext = CriarDbContext();
        var repository = new CreditoRepository(dbContext);

        await repository.AddAsync(CriarCredito("123456", "7891011", new DateOnly(2024, 2, 25)), CancellationToken.None);

        var result = await repository.GetByNumeroCreditoAsync("123456", CancellationToken.None);

        result.Should().NotBeNull();
        result!.NumeroNfse.Should().Be("7891011");
    }

    private static CreditoDbContext CriarDbContext()
    {
        var options = new DbContextOptionsBuilder<CreditoDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new CreditoDbContext(options);
    }

    private static Credito CriarCredito(string numeroCredito, string numeroNfse, DateOnly dataConstituicao)
    {
        return Credito.Criar(
            numeroCredito,
            numeroNfse,
            dataConstituicao,
            1500.75m,
            "ISSQN",
            true,
            5.0m,
            30000.00m,
            5000.00m,
            25000.00m);
    }
}
