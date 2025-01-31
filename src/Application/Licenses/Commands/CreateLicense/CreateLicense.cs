using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Domain.Entities;

namespace AIExtensionsCenter.Application.Licenses.Commands.CreateLicense;

public record CreateLicenseCommand : IRequest<Guid>
{
    public DateTime ExpirationDate { get; init; }
    public bool IsActive { get; init; }
}

public class CreateLicenseCommandValidator : AbstractValidator<CreateLicenseCommand>
{
    public CreateLicenseCommandValidator()
    {
        //RuleFor(x => x.ExtensionId).NotEmpty();
        RuleFor(x => x.ExpirationDate)
            .GreaterThan(DateTime.UtcNow.AddDays(1))
            .WithMessage("Expiration date must be at least 1 day in the future");
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
        License? license = new()
        {
            LicenseKey = Guid.NewGuid().ToString(),
            ExpirationDate = request.ExpirationDate,
            IsActive = request.IsActive,
        };

        _context.Licenses.Add(license);
        await _context.SaveChangesAsync(cancellationToken);

        return license.Id;
    }
}
