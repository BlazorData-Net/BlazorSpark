using Aspire.Hosting;
using k8s.Models;
using Microsoft.Extensions.DependencyInjection;

var builder = DistributedApplication.CreateBuilder(args);

// Add blobs
var blobStorage = builder.AddAzureStorage("blobStorage")
                         .RunAsEmulator()
                         .AddBlobs("blobs");

// Add the UI‑container instance with a uniquely named HTTPS endpoint
var webContainer = builder.AddProject<Projects.BlazorUIContainer>("webcontainer")
    .WithReference(blobStorage)
    .WithExternalHttpEndpoints();

// Add the web frontend — reference the same storage for both queue and blob access
builder.AddProject<Projects.BlazorSpark_Web>("webfrontend")
       .WithReference(blobStorage)
       .WithExternalHttpEndpoints();

builder.Build().Run();