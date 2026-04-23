using Amazon.SQS;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payments.Application.IntegrationEvents;
using Payments.Domain.Interfaces;
using Payments.Infrastructure.Messaging;
using Payments.Infrastructure.Messaging.Sqs;
using Payments.Infrastructure.Persistence;
using Payments.Infrastructure.Persistence.Repositories;

namespace Payments.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // ── Banco de Dados ────────────────────────────────────────────────────
        services.AddDbContext<PaymentsDbContext>(opts =>
            opts.UseNpgsql(configuration.GetConnectionString("Payments")));

        services.AddScoped<IPaymentRepository, PaymentRepository>();

        // ── Mensageria — SQS (produção AWS) ───────────────────────────────────
        var region = configuration["AWS:Region"];
        if (!string.IsNullOrWhiteSpace(region))
        {
            services.AddSingleton<IAmazonSQS>(_ =>
                new AmazonSQSClient(Amazon.RegionEndpoint.GetBySystemName(region)));
            services.AddSingleton<ISqsPublisher, SqsPublisher>();
        }
        else
        {
            services.AddSingleton<ISqsPublisher, NoopSqsPublisher>();
        }
        services.AddScoped<IEventPublisher, SqsEventPublisher>();

        // ── Mensageria — RabbitMQ (desenvolvimento local) ─────────────────────
        services.AddMassTransit(x =>
        {
            x.AddConsumer<OrderPlacedConsumer>();
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(configuration["RabbitMQ:Host"], "/", h =>
                {
                    h.Username(configuration["RabbitMQ:Username"] ?? throw new InvalidOperationException("RabbitMQ:Username is missing."));
                    h.Password(configuration["RabbitMQ:Password"] ?? throw new InvalidOperationException("RabbitMQ:Password is missing."));
                });
                cfg.ConfigureEndpoints(ctx);
            });
        });

        return services;
    }
}
