using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Application.Common.Mappings;
using AIExtensionsCenter.Application.Common.Models;

namespace AIExtensionsCenter.Application.Extensions.Queries.GetExtension;

public record GetExtensionQuery : IRequest<PaginatedList<ExtensionVM>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetExtensionQueryValidator : AbstractValidator<GetExtensionQuery>
{
    public GetExtensionQueryValidator()
    {
    }
}

public class GetExtensionQueryHandler : IRequestHandler<GetExtensionQuery, PaginatedList<ExtensionVM>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetExtensionQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ExtensionVM>> Handle(GetExtensionQuery request, CancellationToken cancellationToken)
    {
        return await _context.Extensions
            .ProjectTo<ExtensionVM>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
