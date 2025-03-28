﻿using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// Add queues
var storage = builder.AddAzureStorage("storage")
                     .RunAsEmulator()
                     .AddQueues("queues");
// Add blobs
var blobStorage = builder.AddAzureStorage("blobStorage")
                         .RunAsEmulator()
                         .AddBlobs("blobs");

// Add the web frontend — reference the same storage for both queue + blob access
builder.AddProject<Projects.BlazorSpark_Web>("webfrontend")
       .WithReference(storage)
       .WithReference(blobStorage)
       .WithExternalHttpEndpoints();

// Add both UI‑container instances, referencing the same storage
builder.AddProject<Projects.BlazorUIContainer>("webcontainer")
       .WithReference(storage)
       .WithReference(blobStorage)
       .WithHttpsEndpoint(name: "webcontainer-http", isProxied: true);

builder.Build().Run();