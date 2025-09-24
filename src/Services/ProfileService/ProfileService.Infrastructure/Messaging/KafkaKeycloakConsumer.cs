using System.Text.Json;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProfileService.Application.Commands;
using ProfileService.Infrastructure.Models;
using ProfileService.Infrastructure.Models.Enums;

namespace ProfileService.Infrastructure.Messaging;

public sealed class KafkaKeycloakConsumer : BackgroundService
{
    private readonly ILogger<KafkaKeycloakConsumer> _logger;
    private readonly IMediator _mediator;
    private readonly ConsumerConfig _config;
    private readonly string _topic;

    public KafkaKeycloakConsumer(IConfiguration cfg, ILogger<KafkaKeycloakConsumer> logger, IMediator mediator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator  ?? throw new ArgumentNullException(nameof(mediator));
        ArgumentNullException.ThrowIfNull(cfg);
        _topic = cfg["Kafka:Topic"] ?? throw new ArgumentNullException("Kafka:Topic");

        _config = new ConsumerConfig
        {
            BootstrapServers = cfg["Kafka:BootstrapServers"] ?? throw new ArgumentNullException("Kafka:BootstrapServers"),
            GroupId = cfg["Kafka:GroupId"] ?? throw new ArgumentNullException("Kafka:GroupId"),
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
        consumer.Subscribe(_topic);

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var cr = consumer.Consume(cancellationToken);
                    var json = cr.Message.Value;
                    KeycloakKafkaMessage? message = null;

                    try
                    {
                        message = JsonSerializer.Deserialize<KeycloakKafkaMessage>(json);
                    }
                    catch
                    {
                        _logger.LogWarning("Не удалось разобрать сообщение от брокера");
                        consumer.Commit(cr);
                        continue;
                    }

                    if (message.EventType != KeycloakEventType.UserRegistered)
                    {
                        consumer.Commit(cr);
                        continue;
                    }
                    
                    var cmd = new CreateProfileCommand(
                        UserId: Guid.Parse(message.UserId),
                        Email: message.Email,
                        FirstName: message.FirstName,
                        MiddleName: message.MiddleName,
                        LastName: message.LastName,
                        EmailVerified: message.EmailVerified);
                    
                    await _mediator.Send(cmd, cancellationToken);

                    consumer.Commit(cr);
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "Consume error");
                    await Task.Delay(1000, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Processing error");
                }
            }
        }
        finally
        {
            consumer.Close();
        }
    }
}
