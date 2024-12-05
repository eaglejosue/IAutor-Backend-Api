using Azure.Storage.Blobs.Models;

namespace IAutor.Api.Services.External;

public interface IAzureBlobServiceClient
{
    Task<string> UploadFileFromBytesAsync(string containerName, string fileNameOrUrl, byte[] bytes);
    Task<string> UploadFileFromStreamAsync(string containerName, string fileNameOrUrl, Stream stream);
    Task<Stream> DownloadFileStreamAsync(string containerName, string fileNameOrUrl);
    Task<byte[]> DownloadFileBytesAsync(string containerName, string fileNameOrUrl);
    Task DeleteFileAsync(string containerName, string fileNameOrUrl);
}

public class AzureBlobServiceClient(BlobServiceClient blobServiceClient) : IAzureBlobServiceClient
{
    private async Task<BlobContainerClient> GetContainerReferenceAsync(string containerName)
    {
        var container = blobServiceClient.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync().ConfigureAwait(false);
        return container;
    }

    public Task<string> UploadFileFromBytesAsync(string containerName, string fileNameOrUrl, byte[] bytes) =>
        UploadFileFromStreamAsync(containerName, fileNameOrUrl, new MemoryStream(bytes));

    public async Task<string> UploadFileFromStreamAsync(string containerName, string fileNameOrUrl, Stream stream)
    {
        var container = await GetContainerReferenceAsync(containerName).ConfigureAwait(false);
        var blob = container.GetBlobClient(fileNameOrUrl);
        await blob.UploadAsync(stream).ConfigureAwait(false);
        return blob.Uri.ToString();
    }

    public async Task<Stream> DownloadFileStreamAsync(string containerName, string fileNameOrUrl)
    {
        var result = await DownloadContentAsync(containerName, fileNameOrUrl).ConfigureAwait(false);
        return result.Content.ToStream();
    }

    public async Task<byte[]> DownloadFileBytesAsync(string containerName, string fileNameOrUrl)
    {
        var result = await DownloadContentAsync(containerName, fileNameOrUrl).ConfigureAwait(false);
        return result.Content.ToArray();
    }

    public async Task DeleteFileAsync(string containerName, string fileNameOrUrl)
    {
       
            var container = await GetContainerReferenceAsync(containerName).ConfigureAwait(false);
            var blob = container.GetBlobClient(fileNameOrUrl);
            await blob.DeleteIfExistsAsync().ConfigureAwait(false);
  
    }

    private async Task<BlobDownloadResult> DownloadContentAsync(string containerName, string fileNameOrUrl)
    {
        var container = await GetContainerReferenceAsync(containerName).ConfigureAwait(false);
        var blob = container.GetBlobClient(fileNameOrUrl);

        if (!await blob.ExistsAsync().ConfigureAwait(false))
            throw new ValidationException($"File {fileNameOrUrl} not exists.");

        return await blob.DownloadContentAsync().ConfigureAwait(false);
    }
}
