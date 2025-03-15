using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Domain.Entities;
using AIExtensionsCenter.Domain.Enums;
using Ardalis.GuardClauses;

namespace AIExtensionsCenter.Application.Licenses.Commands.ValidateLicense;

public record ValidateLicenseCommand : IRequest
{
    public Guid ExtensionId { get; init; }
    public string LicenseKey { get; init; } = null!;
    public string? HwId { get; init; }
}

public class ValidateLicenseCommandValidator : AbstractValidator<ValidateLicenseCommand>
{
    public ValidateLicenseCommandValidator()
    {
        RuleFor(x => x.ExtensionId)
           .NotEmpty();
        RuleFor(x => x.LicenseKey)
           .NotEmpty();
        RuleFor(x => x.HwId)
           .NotEmpty();
    }
}

public class ValidateLicenseCommandHandler : IRequestHandler<ValidateLicenseCommand>
{
    private readonly IApplicationDbContext _context;

    public ValidateLicenseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ValidateLicenseCommand request, CancellationToken cancellationToken)
    {
        License? license = await _context.Licenses.FirstOrDefaultAsync(x => x.LicenseKey == request.LicenseKey && x.ExtensionId == request.ExtensionId);
        Guard.Against.NotFound(request.LicenseKey, license);
        if (license.ActivatedMachineId != request.HwId) throw new ValidationException("License invalid");
        if (license.LicenseStatus != LicenseStatus.Active) throw new ValidationException("License invalid");
    }
}
