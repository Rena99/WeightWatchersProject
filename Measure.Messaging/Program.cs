using AutoMapper;
using Measure.Data;
using Measure.Services;
using Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Measure.Messaging
{
    class Program
    {
        public static async Task Main()
        {
            Console.Title = "Measure";

            var endpointConfiguration = new EndpointConfiguration("Measure");
            var containerSettings = endpointConfiguration.UseContainer(new DefaultServiceProviderFactory());
            containerSettings.ServiceCollection.AddScoped<IMeasureService, MeasureService>();
            containerSettings.ServiceCollection.AddScoped<IMeasureRepository, MeasureRepository>();
            containerSettings.ServiceCollection.AddDbContext<MeasureContext>(options =>
                options.UseSqlServer(ConfigurationManager.AppSettings["MeasureDB"]));
            containerSettings.ServiceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");
            //endpointConfiguration.AuditSagaStateChanges(
            //        serviceControlQueue: "Particular.weightwatchers");
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
            routing.RouteToEndpoint(assembly: typeof(AddTracking).Assembly, destination: "Tracking");
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
