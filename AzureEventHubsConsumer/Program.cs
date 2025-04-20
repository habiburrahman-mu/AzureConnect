using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using System.Text;

string storageConnectionString = "<AZURE_STORAGE_CONNECTION_STRING>";
string blobContainerName = "<BLOB_CONTAINER_NAME>";

// Create a blob container client that the event processor will use 
BlobContainerClient storageClient = new BlobContainerClient(
    storageConnectionString, blobContainerName);

string eventHubsNamespaceConnectionString = "<EVENT_HUBS_NAMESPACE_CONNECTION_STRING>";
string eventHubName = "<HUB_NAME>";

// Create an event processor client to process events in the event hub
var processor = new EventProcessorClient(
    storageClient,
    EventHubConsumerClient.DefaultConsumerGroupName,
    eventHubsNamespaceConnectionString,
    eventHubName);

// Register handlers for processing events and handling errors
processor.ProcessEventAsync += ProcessEventHandler;
processor.ProcessErrorAsync += ProcessErrorHandler;

// Start the processing
await processor.StartProcessingAsync();

// Wait for 30 seconds for the events to be processed
await Task.Delay(TimeSpan.FromSeconds(30));

// Stop the processing
await processor.StopProcessingAsync();

Task ProcessEventHandler(ProcessEventArgs eventArgs)
{
    // Write the body of the event to the console window
    Console.WriteLine("\tReceived event: {0}", Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()));
    return Task.CompletedTask;
}

Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
{
    // Write details about the error to the console window
    Console.WriteLine($"\tPartition '{eventArgs.PartitionId}': an unhandled exception was encountered. This was not expected to happen.");
    Console.WriteLine(eventArgs.Exception.Message);
    return Task.CompletedTask;
}