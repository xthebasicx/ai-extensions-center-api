using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Application.Common.Mappings;
using AIExtensionsCenter.Application.Common.Models;

namespace AIExtensionsCenter.Application.Extensions.Queries.GetExtension;

public record GetExtensionWithPaginationQuery : IRequest<PaginatedList<ExtensionVM>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetExtensionWithPaginationQueryValidator : AbstractValidator<GetExtensionWithPaginationQuery>
{
    public GetExtensionWithPaginationQueryValidator()
    {
    }
}

public class GetExtensionWithPaginationQueryHandler : IRequestHandler<GetExtensionWithPaginationQuery, PaginatedList<ExtensionVM>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IFileStorageService _fileStorage;

    public GetExtensionWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper,IFileStorageService fileStorage)
    {
        _context = context;
        _mapper = mapper;
        _fileStorage = fileStorage;
    }

    public async Task<PaginatedList<ExtensionVM>> Handle(GetExtensionWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var extensions = await _context.Extensions
        .OrderBy(e => e.Id)
        .ProjectTo<ExtensionVM>(_mapper.ConfigurationProvider)
        .PaginatedListAsync(request.PageNumber, request.PageSize);

        var tasks = extensions.Items
            .Where(x => !string.IsNullOrEmpty(x.ImageUrl))
            .Select(async x => x.ImageUrl = await _fileStorage.GetPresignedUrlAsync(x.ImageUrl!));

        await Task.WhenAll(tasks);

        return extensions;
    }
}
