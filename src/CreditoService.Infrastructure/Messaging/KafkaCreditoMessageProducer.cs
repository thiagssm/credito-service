using System.Text.Json;
using Confluent.Kafka;
using CreditoService.Application.Abstractions.Messaging;
using CreditoService.Application.DTOs.CreditoConstituido;
using Microsoft.Extensions.Options;

namespace CreditoService.Infrastructure.Messaging;

public sealed class KafkaCreditoMessageProducer : ICreditoMessageProducer, IDisposable
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly IProducer<Null, string> _producer;
    private readonly KafkaOptions _options;

    public KafkaCreditoMessageProducer(IOptions<KafkaOptions> options)
    {
        _options = options.Value;
        var config = new ProducerConfig
        {
            BootstrapServers = _options.BootstrapServers,
            Acks = Acks.All
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task PublishAsync(CreditoConstituidoInputModel credito, CancellationToken cancellationToken)
    {
        var payload = JsonSerializer.Serialize(credito, JsonOptions);

        await _producer.ProduceAsync(
            _options.Topic,
            new Message<Null, string> { Value = payload },
            cancellationToken);
    }

    public void Dispose()
    {
        _producer.Flush(TimeSpan.FromSeconds(5));
        _producer.Dispose();
    }
}
