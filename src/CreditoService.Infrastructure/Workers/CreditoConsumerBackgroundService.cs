using CreditoService.Application.Abstractions.Messaging;
using CreditoService.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CreditoService.Infrastructure.Workers;

public sealed class CreditoConsumerBackgroundService : BackgroundService
{
    private static readonly TimeSpan PollInterval = TimeSpan.FromMilliseconds(500);
    private readonly ICreditoMessageConsumer _creditoMessageConsumer;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<CreditoConsumerBackgroundService> _logger;

    public CreditoConsumerBackgroundService(
        ICreditoMessageConsumer creditoMessageConsumer,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<CreditoConsumerBackgroundService> logger)
    {
        _creditoMessageConsumer = creditoMessageConsumer;
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var message = _creditoMessageConsumer.Consume(PollInterval);
                if (message is null)
                {
                    await Task.Delay(PollInterval, stoppingToken);
                    continue;
                }

                using var scope = _serviceScopeFactory.CreateScope();
                var processor = scope.ServiceProvider.GetRequiredService<ICreditoMessageProcessor>();

                await processor.ProcessAsync(message.Credito, stoppingToken);
                _creditoMessageConsumer.Commit(message);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Erro ao consumir mensagem de credito constituido.");
                await Task.Delay(PollInterval, stoppingToken);
            }
        }
    }
}
