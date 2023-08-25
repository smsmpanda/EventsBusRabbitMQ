using Event.Worker.Events;
using Event.Worker.Handle;
using EventBus.Abstractions;
using Microsoft.Extensions.Hosting;

namespace Event.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IEventBus _eventBus;

    public Worker(ILogger<Worker> logger,IEventBus eventBus)
    {
        _logger = logger;
        _eventBus = eventBus;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _eventBus.Subscribe<PriceUpdateIntergrationEvent, PriceUpdateIntergrationEventHandle>();
        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        while (!stoppingToken.IsCancellationRequested) 
        {
            _eventBus.Publish(new PriceUpdateIntergrationEvent { CurrentPrice = 99.9f,OriginalPrice= 199.0f });
            await Task.Delay(1000);
        }
    }
}
