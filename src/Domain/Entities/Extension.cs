namespace AIExtensionsCenter.Domain.Entities;
public class Extension : BaseAuditableEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public int DownloadCount { get; set; } = 0;
    public int ViewCount { get; set; } = 0;
    public string? ModuleName { get; set; }

    // Relationships
    public ICollection<License> Licenses { get; set; } = null!;
    public string UserId { get; set; } = null!;
}
