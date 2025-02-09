using Azure.Storage.Blobs;
using AzureBlobStorageAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobStorageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobController : ControllerBase
    {
        private readonly IBlobConnectorService blobConnectorService;

        public BlobController(IBlobConnectorService blobConnectorService)
        {
            this.blobConnectorService = blobConnectorService;
        }

        [HttpGet("List/{containerName}")]
        public async Task<ActionResult<List< string>>> List(string containerName)
        {
            BlobServiceClient blobServiceClient = blobConnectorService.GetBlobServiceClient();
            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var containerExists = await blobContainerClient.ExistsAsync();
            if (containerExists.Value)
            {
                var blobsResult = blobContainerClient.GetBlobsAsync();
                var resultList = new List<string>();
                await foreach(var blobMeta in blobsResult)
                {
                    resultList.Add(blobMeta.Name);
                }

                return Ok(resultList);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("Blob/{path}")]
        public async Task<ActionResult> GetBlob(string path)
        {

        }
    }
}
