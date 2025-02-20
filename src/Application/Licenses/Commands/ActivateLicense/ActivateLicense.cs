using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Domain.Entities;
using Ardalis.GuardClauses;

namespace AIExtensionsCenter.Application.Licenses.Commands.ActivateLicense;

public record ActivateLicenseCommand : IRequest
{
    public Guid Id { get; init; }
    public string LicenseKey { get; init; } = null!;
}

public class ActivateLicenseCommandValidator : AbstractValidator<ActivateLicenseCommand>
{
    public ActivateLicenseCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
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

        License? license = await _context.Licenses.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        Guard.Against.NotFound(request.Id, license);

        if (license.LicenseKey != request.LicenseKey) throw new ValidationException("License key invalid");

        license.IsActive = true;
        license.ActivationDate = DateTime.UtcNow;
        license.ActivatedByUserId = userId;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
