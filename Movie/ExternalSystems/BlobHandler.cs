using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Movie.ExternalSystems.Contracts;

namespace Movie.ExternalSystems;

public class BlobHandler: IBlobHandler
{
    private readonly ILogger<BlobHandler> _logger;
    private readonly BlobServiceClient _blobClient;

    public BlobHandler(
        ILogger<BlobHandler> logger,
        BlobServiceClient blobClient)
    {
        _logger = logger;
        _blobClient = blobClient;
    }


    public async Task<string> UploadAsync(string containerName, string blobName, byte[] content, string contentType)
    {
        BlobClient blob = _blobClient
            .GetBlobContainerClient(containerName)
            .GetBlobClient(blobName);

        await blob.UploadAsync(new BinaryData(content), new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
        });

        _logger.LogInformation("Uploaded {Container}/{Blob} ({Size} bytes)", containerName, blobName, content.Length);

        return blob.Uri.ToString();
    }

    public async Task<bool> DeleteAsync(string url, string containerName)
    {
        BlobUriBuilder parts = new(new Uri(url));

        bool deleted = await _blobClient
            .GetBlobContainerClient(parts.BlobContainerName)
            .GetBlobClient(parts.BlobName)
            .DeleteIfExistsAsync();

        if (deleted)
            _logger.LogInformation("Deleted {Url}", url);

        return deleted;
    }

    public Task<string> GetUrlAsync(string containerName, string blobName)
    {
        throw new NotImplementedException();
    }
}