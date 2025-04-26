using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Application.Common.Models;
using AIExtensionsCenter.Domain.Entities;
using Ardalis.GuardClauses;
using Org.BouncyCastle.Asn1.X509;

namespace AIExtensionsCenter.Application.Extensions.Queries.GetExtensionByUserId;

public record GetExtensionByUserIdQuery : IRequest<List<ExtensionVM>>
{
}

public class GetExtensionByUserIdQueryValidator : AbstractValidator<GetExtensionByUserIdQuery>
{
    public GetExtensionByUserIdQueryValidator()
    {
    }
}

public class GetExtensionByUserIdQueryHandler : IRequestHandler<GetExtensionByUserIdQuery, List<ExtensionVM>>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;
    private readonly IMapper _mapper;
    private readonly IFileStorageService _fileStorage;

    public GetExtensionByUserIdQueryHandler(IApplicationDbContext context, IUser user, IMapper mapper, IFileStorageService fileStorageService)
    {
        _context = context;
        _user = user;
        _mapper = mapper;
        _fileStorage = fileStorageService;
    }

    public async Task<List<ExtensionVM>> Handle(GetExtensionByUserIdQuery request, CancellationToken cancellationToken)
    {
        var userId = _user.Id ?? throw new UnauthorizedAccessException();

        List<ExtensionVM> extensions = await _context.Extensions
        .Where(x => x.UserId == userId)
        .ProjectTo<ExtensionVM>(_mapper.ConfigurationProvider)
        .ToListAsync(cancellationToken);

        Guard.Against.NotFound(userId, extensions);

        var tasks = extensions
        .Where(x => !string.IsNullOrEmpty(x.ImageUrl))
        .Select(async x => x.ImageUrl = await _fileStorage.GetPresignedUrlAsync(x.ImageUrl!));

        await Task.WhenAll(tasks);

        return extensions;
    }
}
