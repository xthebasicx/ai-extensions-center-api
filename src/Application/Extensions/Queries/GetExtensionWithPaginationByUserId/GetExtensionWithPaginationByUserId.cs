using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Application.Common.Mappings;
using AIExtensionsCenter.Application.Common.Models;

namespace AIExtensionsCenter.Application.Extensions.Queries.GetExtensionByUserId;

public record GetExtensionWithPaginationByUserIdQuery : IRequest<PaginatedList<ExtensionVM>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetExtensionWithPaginationByUserIdQueryValidator : AbstractValidator<GetExtensionWithPaginationByUserIdQuery>
{
    public GetExtensionWithPaginationByUserIdQueryValidator()
    {
    }
}

public class GetExtensionWithPaginationByUserIdQueryHandler : IRequestHandler<GetExtensionWithPaginationByUserIdQuery, PaginatedList<ExtensionVM>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IUser _user;
    private readonly IFileStorageService _fileStorage;

    public GetExtensionWithPaginationByUserIdQueryHandler(IApplicationDbContext context, IMapper mapper, IUser user, IFileStorageService fileStorage)
    {
        _context = context;
        _mapper = mapper;
        _user = user;
        _fileStorage = fileStorage;
    }

    public async Task<PaginatedList<ExtensionVM>> Handle(GetExtensionWithPaginationByUserIdQuery request, CancellationToken cancellationToken)
    {
        var userId = _user.Id ?? throw new UnauthorizedAccessException();

        var extensions = await _context.Extensions
            .Where(e => e.UserId == userId)
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
