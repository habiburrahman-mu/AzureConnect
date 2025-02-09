using Microsoft.Azure.Cosmos;

namespace AzureCosmosDbConnect.Infrastructure
{
    public class DbProvider
    {
        private const string _accountEndPoint = ""; // TODO: Add access end point
        private const string _authKey = ""; // TODO: Add auth key here
        private const string _dbName = "ToDo";
        private CosmosClient _cosmosClient { get; set; }
        public DbProvider()
        {
            _cosmosClient = new CosmosClient(_accountEndPoint, _authKey);
        }

        public async Task<Database> GetDatabaseAsync()
        {
            var databaseResponse = await _cosmosClient.CreateDatabaseIfNotExistsAsync(_dbName);
            return databaseResponse.Database;
        }
    }
}
