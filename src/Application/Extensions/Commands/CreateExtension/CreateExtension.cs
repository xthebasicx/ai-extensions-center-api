using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Domain.Entities;

namespace AIExtensionsCenter.Application.Extensions.Commands.CreateExtension;

public record CreateExtensionCommand : IRequest<Guid>
{
    public string ExtensionName { get; init; } = null!;
    public string? Description { get; init; }

}

public class CreateExtensionCommandValidator : AbstractValidator<CreateExtensionCommand>
{
    public CreateExtensionCommandValidator()
    {
        RuleFor(x => x.ExtensionName)
            .NotEmpty().WithMessage("Extension name is required")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters");

        RuleFor(x => x.Description)
            .MaximumLength(150).WithMessage("Description cannot exceed 150 characters");

        //RuleFor(x => x.UserId)
        //    .NotEmpty().WithMessage("User ID is required");
    }
}

public class CreateExtensionCommandHandler : IRequestHandler<CreateExtensionCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateExtensionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateExtensionCommand request, CancellationToken cancellationToken)
    {
        Extension extension = new()
        {
            ExtensionName = request.ExtensionName,
            Description = request.Description,
            //UserId = request.UserId
        };

        _context.Extensions.Add(extension);
        await _context.SaveChangesAsync(cancellationToken);

        return extension.Id;
    }
}
