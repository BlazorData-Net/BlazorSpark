using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// Enable Storage and Queue services.
var queues = builder.AddAzureStorage("storage")
    .RunAsEmulator()
    .AddQueues("queues");

// Add the web frontend project.
builder.AddProject<Projects.BlazorSpark_Web>("webfrontend")
    .WithReference(queues)
    .WithExternalHttpEndpoints();

// Add the first instance of the web container.
builder.AddProject<Projects.BlazorUIContainer>("webcontainer1")
       .WithReference(queues)
       .WithHttpsEndpoint(name: "webcontainer1-http", isProxied: true);

// Add the second instance of the web container.
builder.AddProject<Projects.BlazorUIContainer>("webcontainer2")
       .WithReference(queues)
       .WithHttpsEndpoint(name: "webcontainer2-http", isProxied: true);

builder.Build().Run();