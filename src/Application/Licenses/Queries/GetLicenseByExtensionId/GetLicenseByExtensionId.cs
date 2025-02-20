using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Application.Common.Mappings;
using AIExtensionsCenter.Application.Common.Models;
using AIExtensionsCenter.Domain.Entities;
using Ardalis.GuardClauses;

namespace AIExtensionsCenter.Application.Licenses.Queries.GetLicenseByExtensionId;

public record GetLicenseByExtensionIdQuery : IRequest<PaginatedList<LicenseVM>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public Guid ExtensionId { get; init; }
}

public class GetLicenseByExtensionIdQueryValidator : AbstractValidator<GetLicenseByExtensionIdQuery>
{
    public GetLicenseByExtensionIdQueryValidator()
    {
    }
}

public class GetLicenseByExtensionIdQueryHandler : IRequestHandler<GetLicenseByExtensionIdQuery, PaginatedList<LicenseVM>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetLicenseByExtensionIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<LicenseVM>> Handle(GetLicenseByExtensionIdQuery request, CancellationToken cancellationToken)
    {
        License? license = await _context.Licenses.FirstOrDefaultAsync(x => x.ExtensionId == request.ExtensionId, cancellationToken);
        Guard.Against.NotFound(request.ExtensionId, license);

        return await _context.Licenses
            .Where(l => l.ExtensionId == request.ExtensionId)
            .ProjectTo<LicenseVM>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
