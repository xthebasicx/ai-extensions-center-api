namespace AIExtensionsCenter.Domain.Entities;
public class License : BaseAuditableEntity
{
    public string LicenseKey { get; set; } = null!;
    public DateTime? ActivationDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsActive { get; set; } = false;

    // Relationships
    public Guid ExtensionId { get; set; }
    public Extension Extension { get; set; } = null!;
    public string? ActivatedByUserId { get; set; }
}
