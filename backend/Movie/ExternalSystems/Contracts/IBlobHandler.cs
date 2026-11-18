using Azure.Storage.Blobs.Models;

namespace Movie.ExternalSystems.Contracts;

public interface IBlobHandler
{
    Task<string> UploadAsync(string containerName, string blobName, byte[] content, string contentType);
    Task<bool> DeleteAsync(string url, string containerName);
    Task<string> GetUrlAsync(string containerName, string blobName);
}