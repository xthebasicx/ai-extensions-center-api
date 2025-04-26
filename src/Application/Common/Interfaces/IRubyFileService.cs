namespace AIExtensionsCenter.Application.Common.Interfaces
{
    public interface IRubyFileService
    {
        Task<byte[]> GenerateRubyFileAsync(string extensionId, string moduleName);
    }
}
