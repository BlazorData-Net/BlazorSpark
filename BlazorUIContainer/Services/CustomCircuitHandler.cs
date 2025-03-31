using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Components.Server.Circuits;

public class CustomCircuitHandler : CircuitHandler
{
    private readonly StorageService _storageService;
    private readonly GUIDService _gUIDService;
    public CustomCircuitHandler(
        StorageService storageService,
        GUIDService gUIDService)
    {
        _storageService = storageService;
        _gUIDService = gUIDService;
    }

    public override async Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        try
        {
            // Remove the object from Azure Storage.
            await _storageService.DeleteBlobAsync(_gUIDService.ContainerName, _gUIDService.BlobName);
        }
        catch 
        {
            // Do nothing its not there
        }

        // Optinally, you can kill the DotNet process so the container goes away.
        // Kill the DotNet process so the container goes away
        // System.Diagnostics.Process.GetCurrentProcess().Kill();

        await base.OnCircuitClosedAsync(circuit, cancellationToken);
    }
}