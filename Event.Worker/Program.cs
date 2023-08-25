using Autofac.Extensions.DependencyInjection;
using Event.Worker;
using Event.Worker.Events;
using Event.Worker.Handle;
using EventBus.Abstractions;

internal class Program
{
    private static async Task Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                IConfiguration configuration = hostContext.Configuration;
                services.AddHostedService<Worker>();
                services.AddEventBus(configuration);
            })
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .Build();


        

        await host.RunAsync();
    }
}