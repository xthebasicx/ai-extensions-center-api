using AIExtensionsCenter.Application.Common.Exceptions;
using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Domain.Entities;
using Ardalis.GuardClauses;

namespace AIExtensionsCenter.Application.Extensions.Commands.UpdateExtension;

public record UpdateExtensionCommand : IRequest
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public string? ImageUrl { get; init; }
    public string? ModuleName { get; init; }
}

public class UpdateExtensionCommandValidator : AbstractValidator<UpdateExtensionCommand>
{
    public UpdateExtensionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Extension name is required")
            .MaximumLength(50).WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.Description)
            .MaximumLength(150).WithMessage("Description cannot exceed 500 characters");

        RuleFor(x => x.ModuleName)
            .NotEmpty();
    }
}

public class UpdateExtensionCommandHandler : IRequestHandler<UpdateExtensionCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStorageService _fileStorage;
    private readonly IUser _user;

    public UpdateExtensionCommandHandler(IApplicationDbContext context, IFileStorageService fileStorage, IUser user)
    {
        _context = context;
        _fileStorage = fileStorage;
        _user = user;
    }

    public async Task Handle(UpdateExtensionCommand request, CancellationToken cancellationToken)
    {
        Extension? extension = await _context.Extensions.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        Guard.Against.NotFound(request.Id, extension);

        if (extension.UserId != _user.Id) throw new ForbiddenAccessException();

        if (!string.IsNullOrEmpty(extension.ImageUrl))
        {
            await _fileStorage.DeleteFileAsync(extension.ImageUrl);
        }

        extension.Name = request.Name;
        extension.Description = request.Description;
        extension.ImageUrl = request.ImageUrl;
        extension.ModuleName = request.ModuleName;

        _context.Extensions.Update(extension);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
