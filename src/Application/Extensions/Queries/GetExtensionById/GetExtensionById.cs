using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Application.Common.Models;
using AIExtensionsCenter.Domain.Entities;
using Ardalis.GuardClauses;

namespace AIExtensionsCenter.Application.Extensions.Queries.GetExtensionById;

public record GetExtensionByIdQuery(Guid Id) : IRequest<ExtensionVM>
{
}

public class GetExtensionByIdQueryValidator : AbstractValidator<GetExtensionByIdQuery>
{
    public GetExtensionByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}

public class GetExtensionByIdQueryHandler : IRequestHandler<GetExtensionByIdQuery, ExtensionVM>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IFileStorageService _fileStorage;

    public GetExtensionByIdQueryHandler(IApplicationDbContext context, IMapper mapper, IFileStorageService fileStorage)
    {
        _context = context;
        _mapper = mapper;
        _fileStorage = fileStorage;
    }

    public async Task<ExtensionVM> Handle(GetExtensionByIdQuery request, CancellationToken cancellationToken)
    {
        Extension? extension = await _context.Extensions.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        Guard.Against.NotFound(request.Id, extension);

        if (!string.IsNullOrEmpty(extension.ImageUrl))
        {
            extension.ImageUrl = await _fileStorage.GetPresignedUrlAsync(extension.ImageUrl);
        }

        return _mapper.Map<ExtensionVM>(extension);
    }
}
