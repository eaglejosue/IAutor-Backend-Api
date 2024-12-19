using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;

namespace IAutor.Api.Services.External;

public interface IAmazonS3StorageManager
{
    Task UploadFileContainerAsync(string prefix, string fileUrl, Stream arquivo);
    Task DeleteFileContainerAsync(string prefix, string fileUrl);
    Task<Stream> GetFileContainerAsync(string prefix, string fileUrl);
    Task<string> CreateTempUrlAsync(string prefix, string fileUrl, DateTime dataExpiracao);
    string GetUlrRoot();
    string GetUlrRootContainer(string prefix);
    Task MoveFileContainerAsync(string prefix, string fileUrlOrigin, string fileUrlDestin);
}

public class AmazonS3StorageManager(
    AmazonS3Client s3Client,
    string bucketName,
    IConfiguration config) : IAmazonS3StorageManager
{
    private async Task CreatBucketAsync()
    {
        var exist = await AmazonS3Util.DoesS3BucketExistV2Async(s3Client, bucketName);
        if (!exist)
        {
            var putBucketRequest = new PutBucketRequest
            {
                BucketName = bucketName,
                UseClientRegion = true
            };

            await s3Client.PutBucketAsync(putBucketRequest);
        }
    }

    public async Task UploadFileContainerAsync(string prefix, string fileUrl, Stream arquivo)
    {
        await CreatBucketAsync();

        var putRequest = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = prefix + "/" + fileUrl,
            InputStream = arquivo,
        };

        await s3Client.PutObjectAsync(putRequest);
    }

    public async Task DeleteFileContainerAsync(string prefix, string fileUrl)
    {
        await CreatBucketAsync();

        var deleteObjectRequest = new DeleteObjectRequest
        {
            BucketName = bucketName,
            Key = prefix + "/" + fileUrl
        };

        await s3Client.DeleteObjectAsync(deleteObjectRequest);
    }

    public async Task<Stream> GetFileContainerAsync(string prefix, string fileUrl)
    {
        await CreatBucketAsync();

        var getObjectRequest = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = prefix + "/" + fileUrl
        };

        var response = await s3Client.GetObjectAsync(getObjectRequest);

        return response.ResponseStream;
    }

    public async Task<string> CreateTempUrlAsync(string prefix, string fileUrl, DateTime dataExpiracao)
    {
        await CreatBucketAsync();

        var request = new GetPreSignedUrlRequest
        {
            BucketName = bucketName,
            Key = prefix + "/" + fileUrl,
            Expires = dataExpiracao
        };

        return s3Client.GetPreSignedURL(request);
    }

    public string GetUlrRoot() => config.GetSection("Aws:S3BucketUrl").Value ?? "https://dev-assets.iautor.com.br/public/";

    public string GetUlrRootContainer(string prefix) => string.Concat(GetUlrRoot(), "/", prefix);

    public async Task MoveFileContainerAsync(string prefix, string fileUrlOrigin, string fileUrlDestin)
    {
        await CreatBucketAsync();

        var getObjectRequest = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = prefix + "/" + fileUrlOrigin
        };

        var response = await s3Client.GetObjectAsync(getObjectRequest);

        using (var stream = response.ResponseStream)
            await UploadFileContainerAsync(prefix, fileUrlDestin, stream);

        await DeleteFileContainerAsync(prefix, fileUrlOrigin);
    }
}