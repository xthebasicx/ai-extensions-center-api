using AIExtensionsCenter.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AIExtensionsCenter.Application.Extensions.Commands.UploadExtensionImage;

public record UploadExtensionImageCommand(IFormFile File) : IRequest<string>
{
}

public class UploadExtensionImageCommandValidator : AbstractValidator<UploadExtensionImageCommand>
{
    public UploadExtensionImageCommandValidator()
    {
        RuleFor(x => x.File).NotNull().WithMessage("File is required.");
        RuleFor(x => x.File.Length).GreaterThan(0).WithMessage("File cannot be empty.");
        RuleFor(x => x.File.ContentType).Must(x => x.StartsWith("image/")).WithMessage("Only image files are allowed.");
    }
}

public class UploadExtensionImageCommandHandler : IRequestHandler<UploadExtensionImageCommand, string>
{
    private readonly IFileStorageService _fileStorage;

    public UploadExtensionImageCommandHandler(IApplicationDbContext context, IFileStorageService fileStorage, IUser user)
    {
        _fileStorage = fileStorage;
    }

    public async Task<string> Handle(UploadExtensionImageCommand request, CancellationToken cancellationToken)
    {
        var imageUrl = await _fileStorage.SaveFileAsync(request.File, "extensions");

        return imageUrl;
    }
}
