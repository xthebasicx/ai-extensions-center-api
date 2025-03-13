namespace AIExtensionsCenter.Application.Common.Models;
public class LicenseVM
{
    public Guid? Id { get; init; }
    public string? LicenseKey { get; init; }
    public DateTime ActivationDate { get; init; }
    public DateTime ExpirationDate { get; init; }
    public string? LicenseStatus { get; init; }
    public Guid ExtensionId { get; init; }
    public string? ActivatedByUserEmail { get; init; }
    public string? ActivatedMachineId { get; init; }
}
