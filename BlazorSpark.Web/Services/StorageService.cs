using Microsoft.Identity.Client.Extensions.Msal;
using Azure.Storage.Queues;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

public class StorageService
{
    private readonly BlobServiceClient _blobClient;

    public StorageService( BlobServiceClient blobClient)
    {
        _blobClient = blobClient;
    }   

    // ─────────── Blob Methods ───────────

    public async Task UploadBlobAsync(string containerName, string blobName, string content)
    {
        var container = _blobClient.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync();

        var blob = container.GetBlobClient(blobName);
        await blob.UploadAsync(BinaryData.FromString(content), overwrite: true);
    }

    public async Task<string> ReadBlobAsync(string containerName, string blobName)
    {
        var container = _blobClient.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync();

        var blob = container.GetBlobClient(blobName);

        var download = await blob.DownloadContentAsync();
        return download.Value.Content.ToString();
    }

    public async Task DeleteBlobAsync(string containerName, string blobName)
    {
        var container = _blobClient.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync();
        var blob = container.GetBlobClient(blobName);
        await blob.DeleteIfExistsAsync();
    }
}
