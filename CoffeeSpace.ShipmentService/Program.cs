using CoffeeSpace.Messages.Shipment.Commands;
using CoffeeSpace.Messages.Shipment.Responses;
using CoffeeSpace.Shared.Extensions;
using CoffeeSpace.ShipmentService.Consumers;
using Confluent.Kafka;
using MassTransit;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddOpenTelemetryWithInstrumentation();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Configuration.AddAzureKeyVault();
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
            kafkaConfigurator.Acks = Acks.All;
            kafkaConfigurator.Host(builder.Configuration["Kafka:Host"]);

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