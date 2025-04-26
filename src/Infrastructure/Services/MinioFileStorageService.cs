using AIExtensionsCenter.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;

namespace AIExtensionsCenter.Infrastructure.Services;
public class MinioFileStorageService : IFileStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;

    public MinioFileStorageService(IMinioClient minioClient, IConfiguration configuration)
    {
        _minioClient = minioClient;
        _bucketName = configuration["MINIO:Bucketname"]!;
    }

    public async Task<string> SaveFileAsync(IFormFile file, string containerName)
    {
        var folder = "ai-extension-center";
        var objectName = $"{folder}/{containerName}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        var bucketExists = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucketName));
        if (!bucketExists)
        {
            await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
        }

        await _minioClient.PutObjectAsync(new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectName)
            .WithStreamData(file.OpenReadStream())
            .WithObjectSize(file.Length)
            .WithContentType(file.ContentType));

        return objectName;
    }
    public async Task DeleteFileAsync(string fileUrl)
    {
        await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(fileUrl));
    }
    public async Task<string> GetPresignedUrlAsync(string objectName, int expiryInSeconds = 86400)
    {
        var args = new PresignedGetObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectName)
            .WithExpiry(expiryInSeconds);

        var url = await _minioClient.PresignedGetObjectAsync(args);
        return url;
    }
}
