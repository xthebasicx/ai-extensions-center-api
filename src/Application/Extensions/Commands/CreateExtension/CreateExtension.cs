using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Domain.Entities;

namespace AIExtensionsCenter.Application.Extensions.Commands.CreateExtension;

public record CreateExtensionCommand : IRequest<Guid>
{
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public string? ImageUrl { get; init; }

}

public class CreateExtensionCommandValidator : AbstractValidator<CreateExtensionCommand>
{
    public CreateExtensionCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Extension name is required")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters");

        RuleFor(x => x.Description)
            .MaximumLength(150).WithMessage("Description cannot exceed 150 characters");
    }
}

public class CreateExtensionCommandHandler : IRequestHandler<CreateExtensionCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public CreateExtensionCommandHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task<Guid> Handle(CreateExtensionCommand request, CancellationToken cancellationToken)
    {
        var userId = _user.Id ?? throw new UnauthorizedAccessException();

        Extension extension = new()
        {
            Name = request.Name,
            Description = request.Description,
            UserId = userId,
            ImageUrl = request.ImageUrl,
        };

        _context.Extensions.Add(extension);
        await _context.SaveChangesAsync(cancellationToken);

        return extension.Id;
    }
}
