using Azure.Storage.Blobs.Models;

namespace IAutor.Api.Services.External;

public interface IAzureBlobServiceClient
{
    Task<string> UploadFileFromBytesAsync(string containerName, string fileName, byte[] bytes);
    Task<string> UploadFileFromStreamAsync(string containerName, string fileName, Stream stream);
    Task<Stream> DownloadFileStreamAsync(string containerName, string fileName);
    Task<byte[]> DownloadFileBytesAsync(string containerName, string fileName);
    Task DeleteFileAsync(string containerName, string fileName);
}

public class AzureBlobServiceClient(BlobServiceClient blobServiceClient) : IAzureBlobServiceClient
{
    private async Task<BlobContainerClient> GetContainerReferenceAsync(string containerName)
    {
        var container = blobServiceClient.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync().ConfigureAwait(false);
        return container;
    }

    public Task<string> UploadFileFromBytesAsync(string containerName, string fileName, byte[] bytes) =>
        UploadFileFromStreamAsync(containerName, fileName, new MemoryStream(bytes));

    public async Task<string> UploadFileFromStreamAsync(string containerName, string fileName, Stream stream)
    {
        var container = await GetContainerReferenceAsync(containerName).ConfigureAwait(false);
        var blob = container.GetBlobClient(fileName);
        await blob.UploadAsync(stream).ConfigureAwait(false);
        return blob.Uri.ToString();
    }

    public async Task<Stream> DownloadFileStreamAsync(string containerName, string fileName)
    {
        var result = await DownloadContentAsync(containerName, fileName).ConfigureAwait(false);
        return result.Content.ToStream();
    }

    public async Task<byte[]> DownloadFileBytesAsync(string containerName, string fileName)
    {
        var result = await DownloadContentAsync(containerName, fileName).ConfigureAwait(false);
        return result.Content.ToArray();
    }

    public async Task DeleteFileAsync(string containerName, string fileName)
    {
        var container = await GetContainerReferenceAsync(containerName).ConfigureAwait(false);
        var blob = container.GetBlobClient(fileName);
        await blob.DeleteIfExistsAsync().ConfigureAwait(false);
    }

    private async Task<BlobDownloadResult> DownloadContentAsync(string containerName, string fileName)
    {
        var container = await GetContainerReferenceAsync(containerName).ConfigureAwait(false);
        var blob = container.GetBlobClient(fileName);

        if (!await blob.ExistsAsync().ConfigureAwait(false))
            throw new ValidationException($"File {fileName} not exists.");

        return await blob.DownloadContentAsync().ConfigureAwait(false);
    }
}
