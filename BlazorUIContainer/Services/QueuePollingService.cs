using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

public class QueuePollingService : BackgroundService
{
    private readonly IServiceProvider _services;
    private const string BlobContainer = "current-container";
    private const string RequestQueueName = "request-queue";
    private const string AvailableQueueName = "available-queue";

    public QueuePollingService(IServiceProvider services) => _services = services;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _services.CreateScope();
            var storage = scope.ServiceProvider.GetRequiredService<StorageService>();
            var state = scope.ServiceProvider.GetRequiredService<WorkState>();

            // Try to read a message from the request queue
            var message = await storage.ReadQueueMessageAsync(RequestQueueName);

            // If a message was read, store it and send it to the available queue
            if (!string.IsNullOrEmpty(message) && message != "null")
            {
                // Get the GUID from the message
                var guid = message.Split(" ")[0];

                // Set the message in the WorkState
                // So the Container knows what GUID it plans to serve
                state.SetMessage(message);

                // Stop the loop
                break;
            }

            // Wait 5 seconds before polling again
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}