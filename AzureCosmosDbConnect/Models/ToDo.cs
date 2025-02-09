using Newtonsoft.Json;

namespace AzureCosmosDbConnect.Models
{
    public class ToDo
    {
        [JsonProperty("id")]
        public string Id { get; set; } = null!;
        [JsonProperty("category")]
        public string Category { get; set; } = null!;
        public string Work { get; set; } = null!;
    }
}
