namespace AIExtensionsCenter.Domain.Entities;
public class Extension : BaseAuditableEntity
{
    public string ExtensionName { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }

    // Relationships
    //public Guid UserId { get; set; }
    //public ApplicationUser User { get; set; } = null!;
    //public List<License> Licenses { get; set; } = new();
}
