using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Domain.Entities;
using AIExtensionsCenter.Domain.Enums;

namespace AIExtensionsCenter.Application.Licenses.Commands.CheckExpirationLicense;

public record CheckExpirationLicenseCommand : IRequest
{
}

public class CheckExpirationLicenseCommandValidator : AbstractValidator<CheckExpirationLicenseCommand>
{
    public CheckExpirationLicenseCommandValidator()
    {
    }
}

public class CheckExpirationLicenseCommandHandler : IRequestHandler<CheckExpirationLicenseCommand>
{
    private readonly IApplicationDbContext _context;

    public CheckExpirationLicenseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(CheckExpirationLicenseCommand request, CancellationToken cancellationToken)
    {
        List<License> licenses = await _context.Licenses
            .Where(x => x.ExpirationDate <= DateTime.UtcNow && x.LicenseStatus == LicenseStatus.Active)
            .ToListAsync(cancellationToken);

        licenses.ForEach(license => license.LicenseStatus = LicenseStatus.Expired);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
