using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Application.Common.Models;
using AIExtensionsCenter.Domain.Entities;
using Ardalis.GuardClauses;

namespace AIExtensionsCenter.Application.APIKeys.Queries.GetAPIKeyByUserId;

public record GetAPIKeyByUserIdQuery : IRequest<APIKeyVM>
{
}

public class GetAPIKeyByUserIdQueryValidator : AbstractValidator<GetAPIKeyByUserIdQuery>
{
    public GetAPIKeyByUserIdQueryValidator()
    {
    }
}

public class GetAPIKeyByUserIdQueryHandler : IRequestHandler<GetAPIKeyByUserIdQuery, APIKeyVM>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IUser _user;

    public GetAPIKeyByUserIdQueryHandler(IApplicationDbContext context, IUser user, IMapper mapper)
    {
        _context = context;
        _user = user;
        _mapper = mapper;
    }

    public async Task<APIKeyVM> Handle(GetAPIKeyByUserIdQuery request, CancellationToken cancellationToken)
    {
        string userId = _user.Id ?? throw new UnauthorizedAccessException();
        APIKey? apikey = await _context.APIKeys.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        Guard.Against.NotFound(userId, apikey);

        return _mapper.Map<APIKeyVM>(apikey);
    }
}
