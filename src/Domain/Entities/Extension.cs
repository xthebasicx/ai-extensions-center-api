namespace AIExtensionsCenter.Domain.Entities;
public class Extension : BaseAuditableEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }

    // Relationships
    public ICollection<License> Licenses { get; set; } = null!;
    public string UserId { get; set; } = null!;
}
