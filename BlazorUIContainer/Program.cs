using BlazorDatasheet.Extensions;
using BlazorUIContainer.Components;
using Azure.Storage.Queues;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Hosting;

namespace BlazorUIContainer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add service defaults & Aspire client integrations.
            builder.AddServiceDefaults();

            // Add Azure Queue & Blob clients.
            builder.AddAzureQueueClient("queues");
            builder.AddAzureBlobClient("blobs");

            // Create StorageService
            builder.Services.AddScoped<StorageService>();

            // Add WorkState (singleton) and background polling
            builder.Services.AddSingleton<WorkState>();
            builder.Services.AddHostedService<QueuePollingService>();

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddBlazorDatasheet();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAntiforgery();
            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}