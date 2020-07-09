using AutoMapper;
using Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using Subscriber.Data;
using Subscriber.Models;
using Subscriber.Services;
using Subscriber.Services.Interfaces;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Subscriber.Messaging
{
    class Program
    {
        public static async Task Main()
        { 
            Console.Title = "Subscriber";

            var endpointConfiguration = new EndpointConfiguration("Subscriber");
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");
            //endpointConfiguration.AuditSagaStateChanges(
            //        serviceControlQueue: "Particular.weightwatchers");
            var containerSettings = endpointConfiguration.UseContainer(new DefaultServiceProviderFactory());
            containerSettings.ServiceCollection.AddScoped<ICardService, CardService>();
            containerSettings.ServiceCollection.AddScoped<ICardRepository, CardRepository>();
            containerSettings.ServiceCollection.AddScoped<ISubscriberServices, SubscriberServices>();
            containerSettings.ServiceCollection.AddScoped<ISubscriberRepository, SubscriberRepository>();
            containerSettings.ServiceCollection.AddDbContext<WeightWatchers>(options =>
                options.UseSqlServer(ConfigurationManager.AppSettings["WeightWatchers"]));
            containerSettings.ServiceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //endpointConfiguration.AuditProcessedMessagesTo("audit");
            //endpointConfiguration.AuditSagaStateChanges(serviceControlQueue: "particular.coronaproject");
            //var defaultFactory = LogManager.Use<DefaultFactory>();
            //defaultFactory.Level(LogLevel.Fatal);
            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Delayed(
                customizations: delayed =>
                {
                    delayed.TimeIncrease(TimeSpan.FromSeconds(1));
                });
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.UseConventionalRoutingTopology();
            transport.ConnectionString(ConfigurationManager.AppSettings["RabbitMQ"]);
            endpointConfiguration.EnableOutbox();
            endpointConfiguration.EnableInstallers();
            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.CacheFor(TimeSpan.FromMinutes(1));
            persistence.SqlDialect<SqlDialect.MsSqlServer>();
            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new SqlConnection(ConfigurationManager.AppSettings["MessagingDB"]);
                });
            var routing = transport.Routing();
            routing.RouteToEndpoint(assembly: typeof(UpdateMeasureStatus).Assembly, destination: "Measure");
            var scanner = endpointConfiguration.AssemblyScanner();
            scanner.ExcludeAssemblies("System.Configuratuion.ConfigurationManager");
            scanner.ThrowExceptions = false;

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
