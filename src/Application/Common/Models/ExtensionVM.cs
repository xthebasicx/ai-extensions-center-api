namespace AIExtensionsCenter.Application.Common.Models;
public class ExtensionVM
{
    public Guid? Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? ImageUrl { get; set; }
    public int DownloadCount { get; init; }
    public int ViewCount { get; init; }
    public string? ModuleName { get; init; }
}
