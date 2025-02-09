using AzureCosmosDbConnect.Infrastructure;
using AzureCosmosDbConnect.Interfaces;
using AzureCosmosDbConnect.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace AzureCosmosDbConnect.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly DbProvider dbProvider;

        public TodoRepository(DbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }

        public async Task<List<ToDo>> GetListAsync()
        {
            var container = await GetContainerAsync();
            var query = container.GetItemLinqQueryable<ToDo>();

            using FeedIterator<ToDo> linqFeed = query.ToFeedIterator();

            var itemList = new List<ToDo>();

            while (linqFeed.HasMoreResults)
            {
                FeedResponse<ToDo> response = await linqFeed.ReadNextAsync();
                var list = response.ToList();
                itemList.AddRange(list);
            }

            return itemList;
        }

        public async Task<ToDo> GetSingleAsync(string id, string partitionKey)
        {
            var container = await GetContainerAsync();
            var itemResponse = container.ReadItemAsync<ToDo>(id: id, partitionKey: new PartitionKey(partitionKey));
            return itemResponse.Result;
        }

        public async Task<ToDo> AddItemAsync(ToDo toDo)
        {
            var container = await GetContainerAsync();
            var itemResponse = await container.CreateItemAsync(toDo, new PartitionKey(toDo.Category));
            return itemResponse.Resource;
        }

        public async Task<ToDo> UpdateItemAsync(ToDo toDo)
        {
            var container = await GetContainerAsync();

            var replacedItem = await container.ReplaceItemAsync(
                    item: toDo,
                    id: toDo.Id,
                    partitionKey: new PartitionKey(toDo.Category)
            );

            return replacedItem;
        }

        public async Task<ToDo> DeleteItemAsync(ToDo toDo)
        {
            var container = await GetContainerAsync();

            var itemResponse = await container.DeleteItemAsync<ToDo>(
                    id: toDo.Id,
                    partitionKey: new PartitionKey(toDo.Category)
                );

            return itemResponse.Resource;
        }

        private async Task<Container> GetContainerAsync()
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

        private string PartitionKey
        {
            get
            {
                return $"/{nameof(ToDo.Category).ToLower()}";
            }
        }
    }
}
