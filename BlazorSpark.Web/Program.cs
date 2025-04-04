using BlazorDatasheet.Extensions;
using BlazorSpark.Web;
using BlazorSpark.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add Azure Blob client
builder.AddAzureBlobClient("blobs");

// Get ContainerURL from Appsettings
var containerUrl = builder.Configuration["ContainerUrl"];

// Create StorageService
builder.Services.AddScoped<StorageService>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();

builder.Services.AddBlazorDatasheet();

var app = builder.Build();

// Set ContainerUrl
app.Configuration["ContainerUrl"] = containerUrl;

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseOutputCache();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();