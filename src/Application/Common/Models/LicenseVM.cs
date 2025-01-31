namespace AIExtensionsCenter.Application.Common.Models;
public class LicenseVM
{
    public Guid? Id { get; init; }
    public string LicenseKey { get; init; } = null!;
    public DateTime ActivationDate { get; init; }
    public DateTime ExpirationDate { get; init; }
    public bool IsActive { get; init; }
}
