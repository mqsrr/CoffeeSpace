using CoffeeSpace.Messages.Shipment.Commands;
using CoffeeSpace.Messages.Shipment.Responses;
using CoffeeSpace.Shared.Extensions;
using CoffeeSpace.Shared.Settings;
using CoffeeSpace.ShipmentService.Consumers;
using Confluent.Kafka;
using MassTransit;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddOpenTelemetryWithInstrumentation();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Configuration.AddAzureKeyVault();

builder.Services.AddOptionsWithValidateOnStart<KafkaSettings>()
    .Bind(builder.Configuration.GetRequiredSection(KafkaSettings.SectionName))
    .Configure(settings => 
        settings.Hosts = JsonConvert.DeserializeObject<IReadOnlyList<string>>(builder.Configuration["Kafka:Hosts"]!)!);

builder.Services.AddMassTransit(x =>
{
    x.UsingInMemory();
    x.AddRider(configurator =>
    {
        configurator.SetKebabCaseEndpointNameFormatter();
        configurator.AddProducer<OrderShipped>("order-shipped");
        
        configurator.AddConsumer<RequestOrderShipmentConsumer>();
        configurator.UsingKafka((context, kafkaConfigurator) =>
        {
            var kafkaSettings = context.GetRequiredService<IOptions<KafkaSettings>>().Value;
            kafkaConfigurator.Acks = Acks.All;
            
            kafkaConfigurator.Host(kafkaSettings.Hosts);
            kafkaConfigurator.AddTopicEndpoint<RequestOrderShipment, RequestOrderShipmentConsumer>(context, "request-order-shipment", "shipment");
        });
    });
});

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.MapPrometheusScrapingEndpoint();

app.UseHealthChecks("/_health");
app.Run();