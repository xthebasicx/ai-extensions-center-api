using AIExtensionsCenter.Application.Common.Exceptions;
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
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Expiration date must be in the future");
    }
}

public class UpdateLicenseCommandHandler : IRequestHandler<UpdateLicenseCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public UpdateLicenseCommandHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task Handle(UpdateLicenseCommand request, CancellationToken cancellationToken)
    {
        License? license = await _context.Licenses.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        Guard.Against.NotFound(request.Id, license);

        if (license.UserId != _user.Id) throw new ForbiddenAccessException();

        license.ExpirationDate = request.ExpirationDate;

        _context.Licenses.Update(license);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
