using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Domain.Entities;
using AIExtensionsCenter.Domain.Enums;
using Ardalis.GuardClauses;

namespace AIExtensionsCenter.Application.Licenses.Commands.CheckLicense;

public record CheckLicenseCommand : IRequest
{
    public Guid ExtensionId { get; init; }
}

public class CheckLicenseCommandValidator : AbstractValidator<CheckLicenseCommand>
{
    public CheckLicenseCommandValidator()
    {
    }
}

public class CheckLicenseCommandHandler : IRequestHandler<CheckLicenseCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public CheckLicenseCommandHandler(IApplicationDbContext context,IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task Handle(CheckLicenseCommand request, CancellationToken cancellationToken)
    {
        var userId = _user.Id ?? throw new UnauthorizedAccessException();
        License? license = await _context.Licenses.FirstOrDefaultAsync(x => x.ActivatedByUserId == userId && x.ExtensionId == request.ExtensionId);

        Guard.Against.Null(license, "License not found.");

        if (license.ExpirationDate <= DateTime.UtcNow)
        {
            license.LicenseStatus = LicenseStatus.Expired;
            await _context.SaveChangesAsync(cancellationToken);
            throw new ValidationException("License has expired.");
        }
        if (license.LicenseStatus == LicenseStatus.Revoked)
        {
            throw new ValidationException("License has revoked.");
        }
    }
}
