using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Domain.Entities;
using Ardalis.GuardClauses;

namespace AIExtensionsCenter.Application.Extensions.Commands.UpdateExtension;

public record UpdateExtensionCommand : IRequest
{
    public Guid Id { get; init; }
    public string ExtensionName { get; init; } = null!;
    public string? Description { get; init; }
}

public class UpdateExtensionCommandValidator : AbstractValidator<UpdateExtensionCommand>
{
    public UpdateExtensionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required");

        RuleFor(x => x.ExtensionName)
            .NotEmpty().WithMessage("Extension name is required")
            .MaximumLength(50).WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.Description)
            .MaximumLength(200).WithMessage("Description cannot exceed 500 characters");
    }
}

public class UpdateExtensionCommandHandler : IRequestHandler<UpdateExtensionCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateExtensionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateExtensionCommand request, CancellationToken cancellationToken)
    {
        Extension? extension = await _context.Extensions.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        Guard.Against.NotFound(request.Id, extension);

        extension.ExtensionName = request.ExtensionName;
        extension.Description = request.Description;

        _context.Extensions.Update(extension);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
