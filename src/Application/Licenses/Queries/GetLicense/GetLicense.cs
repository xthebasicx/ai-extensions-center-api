using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Application.Common.Mappings;
using AIExtensionsCenter.Application.Common.Models;

namespace AIExtensionsCenter.Application.Licenses.Queries.GetLicense;

public record GetLicenseQuery : IRequest<PaginatedList<LicenseVM>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetLicenseQueryValidator : AbstractValidator<GetLicenseQuery>
{
    public GetLicenseQueryValidator()
    {
    }
}

public class GetLicenseQueryHandler : IRequestHandler<GetLicenseQuery, PaginatedList<LicenseVM>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetLicenseQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<LicenseVM>> Handle(GetLicenseQuery request, CancellationToken cancellationToken)
    {
        return await _context.Licenses
            .ProjectTo<LicenseVM>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
