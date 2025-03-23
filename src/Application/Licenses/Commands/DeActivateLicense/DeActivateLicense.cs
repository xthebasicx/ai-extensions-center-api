using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Domain.Entities;
using AIExtensionsCenter.Domain.Enums;
using Ardalis.GuardClauses;

namespace AIExtensionsCenter.Application.Licenses.Commands.DeActivateLicense;

public record DeActivateLicenseCommand(Guid Id) : IRequest
{
}

public class DeActivateLicenseCommandValidator : AbstractValidator<DeActivateLicenseCommand>
{
    public DeActivateLicenseCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

public class DeActivateLicenseCommandHandler : IRequestHandler<DeActivateLicenseCommand>
{
    private readonly IApplicationDbContext _context;

    public DeActivateLicenseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeActivateLicenseCommand request, CancellationToken cancellationToken)
    {
        License? license = await _context.Licenses.FirstOrDefaultAsync(x => x.Id == request.Id);
        Guard.Against.NotFound(request.Id, license);

        if (license.LicenseStatus != LicenseStatus.Active) throw new ValidationException("License invalid");

        license.LicenseStatus = LicenseStatus.Revoked;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
