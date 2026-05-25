using CreditoService.Application.DTOs.CreditoConstituido;

namespace CreditoService.Application.Services;

public interface ICreditoMessageProcessor
{
    Task ProcessAsync(CreditoConstituidoInputModel inputModel, CancellationToken cancellationToken);
}
