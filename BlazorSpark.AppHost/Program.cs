var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.BlazorSpark_Web>("webfrontend")
    .WithExternalHttpEndpoints();

builder.Build().Run();
