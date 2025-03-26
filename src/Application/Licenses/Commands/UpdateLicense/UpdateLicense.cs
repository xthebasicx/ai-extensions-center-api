using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Domain.Entities;
using Ardalis.GuardClauses;

namespace AIExtensionsCenter.Application.Licenses.Commands.UpdateLicense;

public record UpdateLicenseCommand : IRequest
{
    public Guid Id { get; init; }
    public DateTime ExpirationDate { get; init; }

}

public class UpdateLicenseCommandValidator : AbstractValidator<UpdateLicenseCommand>
{
    public UpdateLicenseCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
        RuleFor(x => x.ExpirationDate)
            .GreaterThan(DateTime.UtcNow);
    }
}

public class UpdateLicenseCommandHandler : IRequestHandler<UpdateLicenseCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateLicenseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateLicenseCommand request, CancellationToken cancellationToken)
    {
        License? license = await _context.Licenses.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        Guard.Against.NotFound(request.Id, license);

        license.ExpirationDate = request.ExpirationDate.ToUniversalTime();

        _context.Licenses.Update(license);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
