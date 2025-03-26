using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Domain.Entities;
using AIExtensionsCenter.Domain.Enums;

namespace AIExtensionsCenter.Application.Licenses.Commands.CreateLicense;

public record CreateLicenseCommand : IRequest<Guid>
{
    public DateTime ExpirationDate { get; init; }
    public Guid ExtensionId { get; init; }
}

public class CreateLicenseCommandValidator : AbstractValidator<CreateLicenseCommand>
{
    public CreateLicenseCommandValidator()
    {
        RuleFor(x => x.ExtensionId).NotEmpty();

        RuleFor(x => x.ExpirationDate)
            .GreaterThan(DateTime.UtcNow);
    }
}

public class CreateLicenseCommandHandler : IRequestHandler<CreateLicenseCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateLicenseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateLicenseCommand request, CancellationToken cancellationToken)
    {
        int licenseCount = await _context.Licenses
            .Where(x => x.ExtensionId == request.ExtensionId && (x.LicenseStatus == LicenseStatus.InActive || x.LicenseStatus == LicenseStatus.Active))
            .CountAsync(cancellationToken);

        if (licenseCount >= 100) throw new ValidationException("This extension has already reached the maximum licenses.");

        string licensekey = Guid.NewGuid().ToString();

        License? license = new()
        {
            LicenseKey = licensekey,
            ExpirationDate = request.ExpirationDate,
            ExtensionId = request.ExtensionId
        };

        _context.Licenses.Add(license);
        await _context.SaveChangesAsync(cancellationToken);

        return license.Id;
    }
}
