using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Domain.Entities;
using Ardalis.GuardClauses;

namespace AIExtensionsCenter.Application.Licenses.Commands.DeleteLicense;

public record DeleteLicenseCommand(Guid Id) : IRequest
{
}

public class DeleteLicenseCommandValidator : AbstractValidator<DeleteLicenseCommand>
{
    public DeleteLicenseCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteLicenseCommandHandler : IRequestHandler<DeleteLicenseCommand>
{
    private readonly IApplicationDbContext _context;
    public DeleteLicenseCommandHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
    }

    public async Task Handle(DeleteLicenseCommand request, CancellationToken cancellationToken)
    {
        License? license = await _context.Licenses.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        Guard.Against.NotFound(request.Id, license);

        //if (license.LicenseStatus != LicenseStatus.InActive) throw new ValidationException("Can delete only inactive license");

        _context.Licenses.Remove(license);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
