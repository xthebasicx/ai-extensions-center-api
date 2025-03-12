using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Domain.Entities;
using AIExtensionsCenter.Domain.Enums;
using Ardalis.GuardClauses;

namespace AIExtensionsCenter.Application.Licenses.Commands.ActivateLicense;

public record ActivateLicenseCommand : IRequest
{
    public string LicenseKey { get; init; } = null!;
}

public class ActivateLicenseCommandValidator : AbstractValidator<ActivateLicenseCommand>
{
    public ActivateLicenseCommandValidator()
    {
    }
}

public class ActivateLicenseCommandHandler : IRequestHandler<ActivateLicenseCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public ActivateLicenseCommandHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task Handle(ActivateLicenseCommand request, CancellationToken cancellationToken)
    {
        var userId = _user.Id ?? throw new UnauthorizedAccessException();
        var userEmail = _user.Email;

        License? license = await _context.Licenses.FirstOrDefaultAsync(x => x.LicenseKey == request.LicenseKey, cancellationToken);
        Guard.Against.NotFound(request.LicenseKey, license);

        if (license.LicenseStatus != LicenseStatus.InActive) throw new ValidationException("License key invalid");

        license.ActivationDate = DateTime.UtcNow;
        license.ActivatedByUserId = userId;
        license.ActivatedByUserEmail = userEmail;
        license.LicenseStatus = LicenseStatus.Active;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
