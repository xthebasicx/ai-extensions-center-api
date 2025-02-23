using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Domain.Entities;

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
            .GreaterThan(DateTime.UtcNow.AddDays(1))
            .WithMessage("Expiration date must be at least 1 day in the future");
    }
}

public class CreateLicenseCommandHandler : IRequestHandler<CreateLicenseCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public CreateLicenseCommandHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task<Guid> Handle(CreateLicenseCommand request, CancellationToken cancellationToken)
    {
        var userId = _user.Id ?? throw new UnauthorizedAccessException();
        var licensekey = Guid.NewGuid().ToString();

        License? license = new()
        {
            LicenseKey = licensekey,
            ExpirationDate = request.ExpirationDate,
            UserId = userId,
            ExtensionId = request.ExtensionId
        };

        _context.Licenses.Add(license);
        await _context.SaveChangesAsync(cancellationToken);

        return license.Id;
    }
}
