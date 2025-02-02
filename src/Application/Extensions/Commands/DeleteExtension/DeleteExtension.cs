using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Domain.Entities;
using Ardalis.GuardClauses;

namespace AIExtensionsCenter.Application.Extensions.Commands.DeleteExtension;

public record DeleteExtensionCommand(Guid Id) : IRequest
{
}

public class DeleteExtensionCommandValidator : AbstractValidator<DeleteExtensionCommand>
{
    public DeleteExtensionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required");
    }
}

public class DeleteExtensionCommandHandler : IRequestHandler<DeleteExtensionCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStorageService _fileStorage;

    public DeleteExtensionCommandHandler(IApplicationDbContext context, IFileStorageService fileStorage)
    {
        _context = context;
        _fileStorage = fileStorage;
    }

    public async Task Handle(DeleteExtensionCommand request, CancellationToken cancellationToken)
    {
        Extension? extension = await _context.Extensions.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        Guard.Against.NotFound(request.Id, extension);

        if (!string.IsNullOrEmpty(extension.ImageUrl))
        {
            await _fileStorage.DeleteFileAsync(extension.ImageUrl);
        }

        _context.Extensions.Remove(extension);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
