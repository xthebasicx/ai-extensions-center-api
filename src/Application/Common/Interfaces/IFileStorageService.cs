using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace AIExtensionsCenter.Application.Common.Interfaces;
public interface IFileStorageService
{
    Task<string> SaveFileAsync(IFormFile file, string containerName);
    Task DeleteFileAsync(string fileUrl);
}
public class LocalFileStorageService : IFileStorageService
{
    private readonly IHostEnvironment _env;

    public LocalFileStorageService(IHostEnvironment env)
    {
        _env = env;
    }
    public async Task<string> SaveFileAsync(IFormFile file, string containerName)
    {
        var uploadsDir = Path.Combine(_env.ContentRootPath, "Uploads", containerName);
        Directory.CreateDirectory(uploadsDir);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploadsDir, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"/{containerName}/{fileName}";
    }
    public Task DeleteFileAsync(string fileUrl)
    {
        var relativePath = fileUrl.TrimStart('/');
        var filePath = Path.Combine(_env.ContentRootPath, "Uploads", relativePath);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        return Task.CompletedTask;
    }
}
