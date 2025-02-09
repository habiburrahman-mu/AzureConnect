using AzureCosmosDbConnect.Interfaces;
using AzureCosmosDbConnect.Models;
using Microsoft.AspNetCore.Mvc;

namespace AzureCosmosDbConnect.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ILogger<TodoController> _logger;
        private readonly ITodoRepository todoRepository;

        public TodoController(ILogger<TodoController> logger, ITodoRepository todoRepository)
        {
            _logger = logger;
            this.todoRepository = todoRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<ToDo>>> GetList()
        {
            var list = await todoRepository.GetListAsync();
            return Ok(list);
        }

        [HttpGet("{id}/{partitionKey}")]
        public async Task<ActionResult<ToDo>> GetDetail(string id, string partitionKey)
        {
            var result = await todoRepository.GetSingleAsync(id, partitionKey);
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<ToDo>> Create([FromBody] ToDo toDo)
        {
            var item = await todoRepository.AddItemAsync(toDo);
            return Ok(item);
        }

        [HttpPut]
        public async Task<ActionResult<ToDo>> Update([FromBody] ToDo toDo)
        {
            var item = await todoRepository.UpdateItemAsync(toDo);
            return Ok(item);
        }

        [HttpDelete]
        public async Task<ActionResult<ToDo>> Delete([FromBody] ToDo toDo)
        {
            var deletedItem = await todoRepository.DeleteItemAsync(toDo);
            return deletedItem;
        }
    }
}
