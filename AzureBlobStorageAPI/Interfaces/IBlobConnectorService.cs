using Azure.Storage.Blobs;

namespace AzureBlobStorageAPI.Interfaces;

public interface IBlobConnectorService
{
    public BlobServiceClient GetBlobServiceClient();
}
