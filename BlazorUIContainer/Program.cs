using BlazorDatasheet.Extensions;
using BlazorUIContainer.Components;
using Azure.Storage.Queues;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace BlazorUIContainer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add service defaults & Aspire client integrations.
            builder.AddServiceDefaults();

            // Add Azure Blob client
            builder.AddAzureBlobClient("blobs");

            // Create StorageService
            builder.Services.AddScoped<StorageService>();

            // Add GUIDService
            builder.Services.AddScoped<GUIDService>();

            // Add a Custom CircuitHandler so we can
            // remove the object from Azure Storage when the circuit is closed.
            builder.Services.AddScoped<CircuitHandler, CustomCircuitHandler>();

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddBlazorDatasheet();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            // To allow this Container app to be iframed in another app
            app.Use(async (context, next) =>
            {
                // How to display the app in an iframe
                context.Response.OnStarting(() =>
                {
                    // Remove X-Frame-Options header
                    context.Response.Headers.Remove("X-Frame-Options");

                    // Remove Content-Security-Policy header
                    context.Response.Headers.Remove("Content-Security-Policy");

                    // Add custom CSP to allow iframing from anywhere
                    context.Response.Headers.Append("Content-Security-Policy",
                        "default-src 'self'; " +
                        "script-src 'self'; " +
                        "style-src 'self' 'unsafe-inline'; " +
                        "img-src 'self' data:; " +
                        "font-src 'self'; " +
                        "frame-ancestors *;");

                    return Task.CompletedTask;
                });

                await next();
            });

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