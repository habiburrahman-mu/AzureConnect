using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureBlobStorageAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AzureBlobStorageAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContainerController : ControllerBase
    {

        private readonly ILogger<ContainerController> _logger;
        private readonly IBlobConnectorService _blobConnectorService;

        public ContainerController(ILogger<ContainerController> logger, IBlobConnectorService blobConnectorService)
        {
            _logger = logger;
            _blobConnectorService = blobConnectorService;
        }

        [HttpGet("List")]
        public async Task<List<string>> List()
        {
            BlobServiceClient blobServiceClient = _blobConnectorService.GetBlobServiceClient();
            var result = blobServiceClient.GetBlobContainersAsync(BlobContainerTraits.Metadata).AsPages();
            var blobContainerItemList = new List<string>();
            await foreach (var page in result)
            {
                foreach (var container in page.Values)
                {
                    blobContainerItemList.Add(container.Name);
                }
            }
            return blobContainerItemList;
        }

        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> Create(string containerName)
        {
            BlobServiceClient blobServiceClient = _blobConnectorService.GetBlobServiceClient();
            var blobContainer = blobServiceClient.GetBlobContainerClient(containerName);
            var isBlobContainerExists = await blobContainer.ExistsAsync();
            if (!isBlobContainerExists.Value)
            {
                var response = await blobServiceClient.CreateBlobContainerAsync(containerName);
                BlobContainerClient blobContainerClient = response.Value;
                return Ok(blobContainerClient.Name);
            }
            else
            {
                return BadRequest($"Blob container {containerName} already exists.");
            }
        }
    }
}
