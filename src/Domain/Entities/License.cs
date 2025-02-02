namespace AIExtensionsCenter.Domain.Entities;
public class License : BaseAuditableEntity
{
    public string LicenseKey { get; set; } = null!;
    public DateTime? ActivationDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsActive { get; set; }

    // Relationships
    public Guid ExtensionId { get; set; }
    public Extension Extension { get; set; } = null!;
    //public string? UserId { get; set; }
    //public ApplicationUser? User { get; set; }
}
