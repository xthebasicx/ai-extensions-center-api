using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Domain.Entities;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Http;

namespace AIExtensionsCenter.Application.Extensions.Commands.UploadExtensionImage;

public record UploadExtensionImageCommand(Guid ExtensionId, IFormFile File) : IRequest<string>
{
}

public class UploadExtensionImageCommandValidator : AbstractValidator<UploadExtensionImageCommand>
{
    public UploadExtensionImageCommandValidator()
    {
        RuleFor(x => x.ExtensionId).NotEmpty().WithMessage("Extension ID is required.");
        RuleFor(x => x.File).NotNull().WithMessage("File is required.");
        RuleFor(x => x.File.Length).GreaterThan(0).WithMessage("File cannot be empty.");
        RuleFor(x => x.File.ContentType).Must(x => x.StartsWith("image/")).WithMessage("Only image files are allowed.");
    }
}

public class UploadExtensionImageCommandHandler : IRequestHandler<UploadExtensionImageCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStorageService _fileStorage;

    public UploadExtensionImageCommandHandler(IApplicationDbContext context, IFileStorageService fileStorage)
    {
        _context = context;
        _fileStorage = fileStorage;
    }

    public async Task<string> Handle(UploadExtensionImageCommand request, CancellationToken cancellationToken)
    {
        Extension? extension = await _context.Extensions.FirstOrDefaultAsync(x => x.Id == request.ExtensionId, cancellationToken);
        Guard.Against.NotFound(request.ExtensionId, extension);

        if (!string.IsNullOrEmpty(extension.ImageUrl))
        {
            await _fileStorage.DeleteFileAsync(extension.ImageUrl);
        }

        var imageUrl = await _fileStorage.SaveFileAsync(request.File, "extensions");

        extension.ImageUrl = imageUrl;
        await _context.SaveChangesAsync(cancellationToken);

        return imageUrl;
    }
}
