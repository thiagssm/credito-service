using CreditoService.Application.DTOs.CreditoConstituido;

namespace CreditoService.Application.Abstractions.Messaging;

public interface ICreditoMessageProducer
{
    Task PublishAsync(CreditoConstituidoInputModel credito, CancellationToken cancellationToken);
}
