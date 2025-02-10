
using AzureCosmosDbConnect.Infrastructure;
using AzureCosmosDbConnect.Models;
using Microsoft.Azure.Cosmos;
using static Microsoft.Azure.Cosmos.Container;

namespace AzureCosmosDbConnect.ChangeFeedProcessors;

public class ToDoChangeFeedProcessor : BackgroundService
{
    private readonly IServiceProvider serviceProvider;

    public ToDoChangeFeedProcessor(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var dbProvider = scope.ServiceProvider.GetRequiredService<DbProvider>();
            Database db = await dbProvider.GetDatabaseAsync();
            Container sourceContainer = await GetContainerAsync(dbProvider);
            Container leaseContainer = await GetLeaseContainerAsync(dbProvider);

            Console.WriteLine($"Started");

            ChangesHandler<ToDo> handleChanges = async (
                    IReadOnlyCollection<ToDo> changes,
                    CancellationToken cancellationToken
                ) =>
            {
                Console.WriteLine($"START\tHandling batch of changes...");
                foreach (var todo in changes)
                {
                    await Console.Out.WriteLineAsync($"Detected Operation: ");
                    await Console.Out.WriteLineAsync($"Id: {todo.Id}");
                    await Console.Out.WriteLineAsync($"Category: {todo.Category}");
                    await Console.Out.WriteLineAsync($"Work: {todo.Work}");
                    await Console.Out.WriteLineAsync($"-------------");
                }
            };

            var builder = sourceContainer.GetChangeFeedProcessorBuilder<ToDo>(
                    processorName: "todoProcessor",
                    onChangesDelegate: handleChanges
                );

            ChangeFeedProcessor processor = builder
                .WithInstanceName("todoChangeProcessor")
                .WithLeaseContainer(leaseContainer)
                .Build();

            await processor.StartAsync();

            Console.WriteLine("Listening for changes...");
        }
    }

    private async Task<Container> GetContainerAsync(DbProvider dbProvider)
    {
        var db = await dbProvider.GetDatabaseAsync();

        ContainerProperties containerProperties = new ContainerProperties
        {
            Id = nameof(ToDo),
            PartitionKeyPath = PartitionKey,
        };

        var containerResponse = await db.CreateContainerIfNotExistsAsync(containerProperties);

        return containerResponse.Container;
    }

    private async Task<Container> GetLeaseContainerAsync(DbProvider dbProvider)
    {
        var db = await dbProvider.GetDatabaseAsync();

        ContainerProperties containerProperties = new ContainerProperties
        {
            Id = $"{nameof(ToDo)}Lease",
            PartitionKeyPath = "/id",
        };

        var containerResponse = await db.CreateContainerIfNotExistsAsync(containerProperties);

        return containerResponse.Container;
    }



    private string PartitionKey
    {
        get
        {
            return $"/{nameof(ToDo.Category).ToLower()}";
        }
    }
}
