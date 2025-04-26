using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Application.Helper;
using AIExtensionsCenter.Domain.Entities;
using AIExtensionsCenter.Domain.Enums;
using Ardalis.GuardClauses;

namespace AIExtensionsCenter.Application.Licenses.Commands.ActivateLicense;

public record ActivateLicenseCommand : IRequest
{
    public Guid ExtensionId { get; init; }
    public string LicenseKey { get; init; } = null!;
    public string? Email { get; init; }
    public string? HwId { get; init; }
}

public class ActivateLicenseCommandValidator : AbstractValidator<ActivateLicenseCommand>
{
    public ActivateLicenseCommandValidator()
    {
        RuleFor(x => x.ExtensionId)
           .NotEmpty();
        RuleFor(x => x.LicenseKey)
           .NotEmpty();
        RuleFor(x => x.Email)
           .NotEmpty();
        RuleFor(x => x.HwId)
           .NotEmpty();
    }
}

public class ActivateLicenseCommandHandler : IRequestHandler<ActivateLicenseCommand>
{
    private readonly IApplicationDbContext _context;

    public ActivateLicenseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ActivateLicenseCommand request, CancellationToken cancellationToken)
    {
        string decryptedLicenseKey = AesHelper.Decrypt(request.LicenseKey);

        License? license = await _context.Licenses.FirstOrDefaultAsync(x => x.LicenseKey == decryptedLicenseKey && x.ExtensionId == request.ExtensionId, cancellationToken);
        Guard.Against.NotFound(decryptedLicenseKey, license);

        if (license.ExpirationDate <= DateTime.UtcNow || license.LicenseStatus != LicenseStatus.InActive) throw new ValidationException("License invalid");

        license.ActivationDate = DateTime.UtcNow;
        license.LicenseStatus = LicenseStatus.Active;
        license.ActivatedByUserEmail = request.Email;
        license.ActivatedMachineId = request.HwId;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
