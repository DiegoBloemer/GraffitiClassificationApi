using Minio;
using Minio.DataModel.Args;

namespace GraffitiClassificationApi.Api.Services;

/// <summary>
/// Implementação do serviço de armazenamento usando MinIO.
/// </summary>
public class MinioStorageService : IStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;
    private readonly string _endpoint;

    public MinioStorageService(IConfiguration configuration)
    {
        _endpoint = configuration["MinIO:Endpoint"]!;
        var accessKey = configuration["MinIO:AccessKey"]!;
        var secretKey = configuration["MinIO:SecretKey"]!;
        var useSSL = bool.Parse(configuration["MinIO:UseSSL"] ?? "false");
        _bucketName = configuration["MinIO:BucketName"]!;

        _minioClient = new MinioClient()
            .WithEndpoint(_endpoint)
            .WithCredentials(accessKey, secretKey)
            .WithSSL(useSSL)
            .Build();
    }

    public async Task EnsureBucketExistsAsync()
    {
        var bucketExistsArgs = new BucketExistsArgs()
            .WithBucket(_bucketName);

        bool found = await _minioClient.BucketExistsAsync(bucketExistsArgs);

        if (!found)
        {
            var makeBucketArgs = new MakeBucketArgs()
                .WithBucket(_bucketName);

            await _minioClient.MakeBucketAsync(makeBucketArgs);

            // Tornar bucket público para leitura
            var policy = $$"""
            {
              "Version": "2012-10-17",
              "Statement": [
                {
                  "Effect": "Allow",
                  "Principal": {"AWS": "*"},
                  "Action": ["s3:GetObject"],
                  "Resource": ["arn:aws:s3:::{{_bucketName}}/*"]
                }
              ]
            }
            """;

            var setPolicyArgs = new SetPolicyArgs()
                .WithBucket(_bucketName)
                .WithPolicy(policy);

            await _minioClient.SetPolicyAsync(setPolicyArgs);
        }
    }

    public async Task<string> UploadFileAsync(IFormFile file, string folder)
    {
        await EnsureBucketExistsAsync();

        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{extension}";
        var objectName = $"{folder}/{fileName}";

        using var stream = file.OpenReadStream();

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectName)
            .WithStreamData(stream)
            .WithObjectSize(file.Length)
            .WithContentType(file.ContentType);

        await _minioClient.PutObjectAsync(putObjectArgs);

        // Retorna URL pública
        var protocol = _endpoint.Contains("localhost") ? "http" : "https";
        return $"{protocol}://{_endpoint}/{_bucketName}/{objectName}";
    }

    public async Task DeleteFileAsync(string fileUrl)
    {
        if (string.IsNullOrEmpty(fileUrl)) return;

        try
        {
            // Extrai o objectName da URL
            // Ex: http://localhost:9000/graffiti-images/occurrences/abc.jpg
            var uri = new Uri(fileUrl);
            var pathParts = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
            
            if (pathParts.Length < 2) return;

            var objectName = string.Join("/", pathParts.Skip(1)); // occurrences/abc.jpg

            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName);

            await _minioClient.RemoveObjectAsync(removeObjectArgs);
        }
        catch
        {
            // Ignora erros de exclusão (arquivo pode não existir)
        }
    }
}
