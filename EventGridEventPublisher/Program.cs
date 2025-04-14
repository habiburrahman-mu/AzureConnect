using Azure;
using Azure.Messaging.EventGrid;

const string topicEndPoint = "<topic-endpoint>";
/* Update the topicEndpoint string constant by setting its value to the Topic
        Endpoint of the Event Grid topic */

const string topicKey = "<topic-key>";
/* Update the topicKey string constant by setting its value to the Topic
        Key of the Event Grid topic*/

/* To create a new variable named "endpoint" of type "Uri", 
        using the "topicEndpoint" string constant as a constructor parameter */
var endpoint = new Uri(topicEndPoint);

/* To create a new variable named "credential" of type "AzureKeyCredential",
        use the "topicKey" string constant as a constructor parameter. */
var credential = new AzureKeyCredential(topicKey);

/* To create a new variable named "client" of type "EventGridPublisherClient", 
       using the "endpoint" and "credential" variables as constructor parameters */
var client = new EventGridPublisherClient(endpoint, credential);

/* To create a new variable named "firstEvent" of type "EventGridEvent",
        and populate that variable with sample data */
var firstEvent = new EventGridEvent(
    subject: $"New Employee: Habibur Rahman",
    eventType: "Employees.Registration.New",
    dataVersion: "1.0",
    data: new
    {
        FullName = "Habibur Rahman",
        Address = "Dhaka, Bangladesh",
    }
);

/* To create a new variable named "secondEvent" of type "EventGridEvent",
        and populate that variable with sample data */
EventGridEvent secondEvent = new EventGridEvent(
    subject: $"New Employee: Alexandre Doyon",
    eventType: "Employees.Registration.New",
    dataVersion: "1.0",
    data: new
    {
        FullName = "Alexandre Doyon",
        Address = "456 College Street, Bow, WA 98107"
    }
);

/* To asynchronously invoke the "EventGridPublisherClient.SendEventAsync"
       method using the "firstEvent" variable as a parameter */
await client.SendEventAsync(firstEvent);
Console.WriteLine("First event published");

/* To asynchronously invoke the "EventGridPublisherClient.SendEventAsync"
   method using the "secondEvent" variable as a parameter */
await client.SendEventAsync(secondEvent);
Console.WriteLine("Second event published");