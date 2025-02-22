using Microsoft.AspNetCore.Http;

namespace AIExtensionsCenter.Application.Common.Interfaces;
public interface IFileStorageService
{
    Task<string> SaveFileAsync(IFormFile file, string containerName);
    Task DeleteFileAsync(string fileUrl);
}
