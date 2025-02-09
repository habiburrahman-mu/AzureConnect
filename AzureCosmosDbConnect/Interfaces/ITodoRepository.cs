using AzureCosmosDbConnect.Models;

namespace AzureCosmosDbConnect.Interfaces
{
    public interface ITodoRepository
    {
        Task<ToDo> AddItemAsync(ToDo toDo);
        Task<ToDo> DeleteItemAsync(ToDo toDo);
        Task<List<ToDo>> GetListAsync();
        Task<ToDo> GetSingleAsync(string id, string partitionKey);
        Task<ToDo> UpdateItemAsync(ToDo toDo);
    }
}