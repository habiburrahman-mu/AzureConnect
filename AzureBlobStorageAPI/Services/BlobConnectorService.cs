using Azure.Identity;
using Azure.Storage.Blobs;
using AzureBlobStorageAPI.Interfaces;

namespace AzureBlobStorageAPI.Services
{
    public class BlobConnectorService : IBlobConnectorService
    {
        public BlobServiceClient GetBlobServiceClient()
        {
            var connectionString = ""; // TODO: Add connection string here
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            return blobServiceClient;
        }
    }
}
