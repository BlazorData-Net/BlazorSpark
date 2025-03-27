using Microsoft.Identity.Client.Extensions.Msal;

using Azure.Storage.Queues;

public class QueueService
{
    private readonly QueueServiceClient _client;
    public QueueService(QueueServiceClient client)
    {
        _client = client;
    }

    // Peek at the next message in the queue.
    public async Task<string> PeekMessageAsync(string queueName)
    {
        try
        {
            // Get a reference to a queue
            var queueClient = _client.GetQueueClient(queueName);

            // Peek at the next message in the queue
            var messages = await queueClient.PeekMessagesAsync();

            // Get the first message
            var message = messages.Value.FirstOrDefault();

            if (message != null)
            {
                // Return the message text
                return message.MessageText;
            }

            return "null";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public async Task<string> ReadMessageAsync(string queueName)
    {
        try
        {
            // Get a reference to a queue
            var queueClient = _client.GetQueueClient(queueName);

            // Ensure the queue exists
            await queueClient.CreateIfNotExistsAsync();

            // Get messages from the queue
            var messages = await queueClient.ReceiveMessagesAsync();

            // Get the first message
            var message = messages.Value.FirstOrDefault();

            if (message != null)
            {
                // Delete the message
                await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);

                // Return the message text
                return message.MessageText;
            }

            return "null";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public async Task SendMessageAsync(string queueName, string message)
    {
        // Get a reference to a queue
        var queueClient = _client.GetQueueClient(queueName);

        // Ensure the queue exists
        await queueClient.CreateIfNotExistsAsync();

        // Send a message to the queue
        await queueClient.SendMessageAsync(message);
    }
}