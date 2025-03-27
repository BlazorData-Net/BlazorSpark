var builder = DistributedApplication.CreateBuilder(args);

// Enable Storage and Queue services.
// See: https://learn.microsoft.com/en-us/dotnet/aspire/storage/azure-storage-queues-integration?tabs=dotnet-cli
var queues = builder.AddAzureStorage("storage")
    .RunAsEmulator()
    .AddQueues("queues");

builder.AddProject<Projects.BlazorSpark_Web>("webfrontend")
    .WithReference(queues)
    .WithExternalHttpEndpoints(); 

builder.Build().Run();
