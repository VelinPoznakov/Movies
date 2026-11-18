using Azure.Storage.Blobs;
using Movie.ExternalSystems;
using Movie.ExternalSystems.Contracts;

namespace Movie.Infrastructure;

public static class AzureBlobStorageConfiguration
{
    public static void AddAzureBlobStorage(this IServiceCollection services,
        IConfiguration configuration)
    {
        string azureConnectionString = configuration.GetValue<string>("ConnectionStrings:AzureBlobStorage")
            ?? throw new Exception("Azure Blob Storage connection string is missing");
        
        services.AddSingleton(new BlobServiceClient(azureConnectionString));
        services.AddScoped<IBlobHandler, BlobHandler>();
    }

    public static async Task EnsureBlobContainersExistsAsync(this WebApplication application,
        IConfiguration configuration)
    {
        BlobServiceClient client = application
            .Services
            .GetRequiredService<BlobServiceClient>();
        
        IEnumerable<string> containers = configuration
            .GetSection("BlobStorage:Containers")
            .GetChildren()
            .Select(c => c.Value!);

        foreach (string container in containers)
            await client.GetBlobContainerClient(container).CreateIfNotExistsAsync();
    }
}