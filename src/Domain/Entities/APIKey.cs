namespace AIExtensionsCenter.Domain.Entities;
public class APIKey : BaseAuditableEntity
{
    public string Key { get; set; } = null!;

    // Relationships
    public string UserId { get; set; } = null!;
}
