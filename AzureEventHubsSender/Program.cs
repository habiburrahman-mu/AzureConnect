using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System.Text;

int numberOfEvents = 10;

var connectionString = "<connection-string>";
var hubName = "<event-hub-name>";

var producerClient = new EventHubProducerClient(connectionString, hubName);

using var eventDataBatch = await producerClient.CreateBatchAsync();

for (int i = 1; i <= numberOfEvents; i++)
{
    if (!eventDataBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes($"Event {i}"))))
    {
        // if it is too large for the batch
        throw new Exception($"Event {i} is too large for the batch and cannot be sent.");
    }
}

try
{
    await producerClient.SendAsync(eventDataBatch);
    Console.WriteLine($"A batch of {numberOfEvents} events has been published.");
    Console.ReadLine();
}
catch (Exception)
{
    await producerClient.DisposeAsync();
}