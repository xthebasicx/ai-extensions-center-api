using Microsoft.AspNetCore.Http;

namespace AIExtensionsCenter.Application.Common.Interfaces;
public interface IFileStorageService
{
    Task<string> SaveFileAsync(IFormFile file, string containerName);
    Task<string> GetPresignedUrlAsync(string objectName, int expiryInSeconds = 3600);
    Task DeleteFileAsync(string fileUrl);
}
