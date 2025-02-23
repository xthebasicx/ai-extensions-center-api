using AIExtensionsCenter.Domain.Enums;

namespace AIExtensionsCenter.Domain.Entities;
public class License : BaseAuditableEntity
{
    public string LicenseKey { get; set; } = null!;
    public DateTime? ActivationDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public LicenseStatus LicenseStatus { get; set; } = LicenseStatus.InActive;

    // Relationships
    public Guid ExtensionId { get; set; }
    public Extension Extension { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string? ActivatedByUserId { get; set; }
    public string? ActivatedByUserEmail { get; set; }
}
