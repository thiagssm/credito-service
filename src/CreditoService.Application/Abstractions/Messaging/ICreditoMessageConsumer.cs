using CreditoService.Application.DTOs.CreditoConstituido;

namespace CreditoService.Application.Abstractions.Messaging;

public interface ICreditoMessageConsumer
{
    IConsumedCreditoMessage? Consume(TimeSpan timeout);
    void Commit(IConsumedCreditoMessage message);
}

public interface IConsumedCreditoMessage
{
    CreditoConstituidoInputModel Credito { get; }
}
