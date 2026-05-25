using System.Text.Json;
using Confluent.Kafka;
using CreditoService.Application.Abstractions.Messaging;
using CreditoService.Application.DTOs.CreditoConstituido;
using Microsoft.Extensions.Options;

namespace CreditoService.Infrastructure.Messaging;

public sealed class KafkaCreditoMessageConsumer : ICreditoMessageConsumer, IDisposable
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly IConsumer<Ignore, string> _consumer;

    public KafkaCreditoMessageConsumer(IOptions<KafkaOptions> options)
    {
        var kafkaOptions = options.Value;
        var config = new ConsumerConfig
        {
            BootstrapServers = kafkaOptions.BootstrapServers,
            GroupId = kafkaOptions.GroupId,
            EnableAutoCommit = false,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        _consumer.Subscribe(kafkaOptions.Topic);
    }

    public IConsumedCreditoMessage? Consume(TimeSpan timeout)
    {
        ConsumeResult<Ignore, string>? result;
        try
        {
            result = _consumer.Consume(timeout);
        }
        catch (ConsumeException exception) when (exception.Error.Code == ErrorCode.UnknownTopicOrPart)
        {
            return null;
        }

        if (result is null)
        {
            return null;
        }

        var credito = JsonSerializer.Deserialize<CreditoConstituidoInputModel>(result.Message.Value, JsonOptions)
            ?? throw new InvalidOperationException("Mensagem de credito constituido invalida.");

        return new KafkaConsumedCreditoMessage(credito, result);
    }

    public void Commit(IConsumedCreditoMessage message)
    {
        if (message is not KafkaConsumedCreditoMessage kafkaMessage)
        {
            throw new ArgumentException("Mensagem consumida nao pertence ao consumidor Kafka.", nameof(message));
        }

        _consumer.Commit(kafkaMessage.Result);
    }

    public void Dispose()
    {
        _consumer.Close();
        _consumer.Dispose();
    }

    private sealed record KafkaConsumedCreditoMessage(
        CreditoConstituidoInputModel Credito,
        ConsumeResult<Ignore, string> Result) : IConsumedCreditoMessage;
}
