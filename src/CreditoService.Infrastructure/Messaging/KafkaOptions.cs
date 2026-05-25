namespace CreditoService.Infrastructure.Messaging;

public sealed class KafkaOptions
{
    public const string SectionName = "Kafka";

    public string BootstrapServers { get; init; } = "localhost:9092";
    public string Topic { get; init; } = "integrar-credito-constituido-entry";
    public string GroupId { get; init; } = "credito-service";
    public bool EnableConsumer { get; init; } = true;
}
