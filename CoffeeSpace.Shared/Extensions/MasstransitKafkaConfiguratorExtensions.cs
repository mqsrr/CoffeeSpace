using Confluent.Kafka;
using MassTransit;
using MassTransit.KafkaIntegration;

namespace CoffeeSpace.Shared.Extensions;

public static class MasstransitKafkaConfiguratorExtensions
{
    public static IKafkaFactoryConfigurator AddTopicEndpoint<TMessage, TConsumer>(
        this IKafkaFactoryConfigurator kafkaConfigurator, 
        IRiderRegistrationContext context,
        string topicName,
        string groupId,
        Action<KafkaTopicOptions>? topicOptions = null)
        where TMessage : class
        where TConsumer : class, IConsumer<TMessage>
    {
        kafkaConfigurator.TopicEndpoint<TMessage>(topicName, groupId, e =>
        {
            e.AutoOffsetReset = AutoOffsetReset.Earliest;

            e.UseNewtonsoftJsonSerializer();
            e.UseNewtonsoftJsonDeserializer();

            e.ConfigureConsumer<TConsumer>(context);
            e.CreateIfMissing(topicOptions);
        });

        return kafkaConfigurator;
    }    
    public static IKafkaFactoryConfigurator AddSagaTopicEndpoint<TMessage, TSagaInstance>(
        this IKafkaFactoryConfigurator kafkaConfigurator, 
        IRiderRegistrationContext context,
        string topic,
        string groupId,
        Action<KafkaTopicOptions>? topicOptions = null)
        where TMessage : class
        where TSagaInstance : class, SagaStateMachineInstance
    {
        kafkaConfigurator.TopicEndpoint<TMessage>(topic, groupId, e =>
        {
            e.AutoOffsetReset = AutoOffsetReset.Earliest;

            e.UseNewtonsoftJsonSerializer();
            e.UseNewtonsoftJsonDeserializer();

            e.ConfigureSaga<TSagaInstance>(context);
            e.CreateIfMissing(topicOptions);
        });

        return kafkaConfigurator;
    }
}