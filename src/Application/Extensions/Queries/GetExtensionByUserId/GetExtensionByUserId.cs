using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Application.Common.Mappings;
using AIExtensionsCenter.Application.Common.Models;

namespace AIExtensionsCenter.Application.Extensions.Queries.GetExtensionByUserId;

public record GetExtensionByUserIdQuery : IRequest<PaginatedList<ExtensionVM>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetExtensionByUserIdQueryValidator : AbstractValidator<GetExtensionByUserIdQuery>
{
    public GetExtensionByUserIdQueryValidator()
    {
    }
}

public class GetExtensionByUserIdQueryHandler : IRequestHandler<GetExtensionByUserIdQuery, PaginatedList<ExtensionVM>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IUser _user;

    public GetExtensionByUserIdQueryHandler(IApplicationDbContext context, IMapper mapper, IUser user)
    {
        _context = context;
        _mapper = mapper;
        _user = user;
    }

    public async Task<PaginatedList<ExtensionVM>> Handle(GetExtensionByUserIdQuery request, CancellationToken cancellationToken)
    {
        var userId = _user.Id ?? throw new UnauthorizedAccessException();

        return await _context.Extensions
            .Where(e => e.UserId == userId)
            .ProjectTo<ExtensionVM>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
