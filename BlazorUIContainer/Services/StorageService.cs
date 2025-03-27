using Microsoft.Identity.Client.Extensions.Msal;
using Azure.Storage.Queues;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

public class StorageService
{
    private readonly QueueServiceClient _queueClient;
    private readonly BlobServiceClient _blobClient;

    public StorageService(QueueServiceClient queueClient, BlobServiceClient blobClient)
    {
        _queueClient = queueClient;
        _blobClient = blobClient;
    }

    // ─────────── Queue Methods ───────────

    public async Task<string> ReadQueueMessageAsync(string queueName)
    {
        var queue = _queueClient.GetQueueClient(queueName);
        await queue.CreateIfNotExistsAsync();

        var messages = await queue.ReceiveMessagesAsync();
        var message = messages.Value.FirstOrDefault();

        if (message is not null)
        {
            await queue.DeleteMessageAsync(message.MessageId, message.PopReceipt);
            return message.MessageText;
        }

        return "null";
    }

    public async Task SendQueueMessageAsync(string queueName, string message)
    {
        var queue = _queueClient.GetQueueClient(queueName);
        await queue.CreateIfNotExistsAsync();
        await queue.SendMessageAsync(message);
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
