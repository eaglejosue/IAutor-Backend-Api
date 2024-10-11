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

public class AmazonS3StorageManager(AmazonS3Client s3Client, string bucketName) : IAmazonS3StorageManager
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

    public async Task UploadFileContainerAsync(string prefix, string caminhoArquivo, Stream arquivo)
    {
        await CreatBucketAsync();

        var putRequest = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = prefix + "/" + caminhoArquivo,
            InputStream = arquivo,
        };

        await s3Client.PutObjectAsync(putRequest);
    }

    public async Task DeleteFileContainerAsync(string prefix, string caminhoArquivo)
    {
        await CreatBucketAsync();

        var deleteObjectRequest = new DeleteObjectRequest
        {
            BucketName = bucketName,
            Key = prefix + "/" + caminhoArquivo
        };

        await s3Client.DeleteObjectAsync(deleteObjectRequest);
    }

    public async Task<Stream> GetFileContainerAsync(string prefix, string caminhoArquivo)
    {
        await CreatBucketAsync();

        var getObjectRequest = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = prefix + "/" + caminhoArquivo
        };

        var response = await s3Client.GetObjectAsync(getObjectRequest);

        return response.ResponseStream;
    }

    public async Task<string> CreateTempUrlAsync(string prefix, string caminhoArquivo, DateTime dataExpiracao)
    {
        await CreatBucketAsync();

        var request = new GetPreSignedUrlRequest
        {
            BucketName = bucketName,
            Key = prefix + "/" + caminhoArquivo,
            Expires = dataExpiracao
        };

        return s3Client.GetPreSignedURL(request);
    }

    public string GetUlrRoot() => string.Concat("https://", bucketName, ".s3.us-east-1.amazonaws.com/");

    public string GetUlrRootContainer(string prefix) => string.Concat(GetUlrRoot(), "/", prefix);
    public async Task MoveFileContainerAsync(string prefix, string caminhoOrigem, string caminhoDestino)
    {
        await CreatBucketAsync();

        var getObjectRequest = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = prefix + "/" + caminhoOrigem
        };

        var response = await s3Client.GetObjectAsync(getObjectRequest);

        using (var stream = response.ResponseStream)
            await UploadFileContainerAsync(prefix, caminhoDestino, stream);

        await DeleteFileContainerAsync(prefix, caminhoOrigem);
    }

    public class StorageManagerConfig
    {
        public static readonly string[] Containers =
        [
            "default",
            "IAutor-files",
        ];
    }
}