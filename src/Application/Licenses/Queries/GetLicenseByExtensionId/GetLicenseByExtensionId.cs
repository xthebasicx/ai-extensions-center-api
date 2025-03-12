using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Application.Common.Models;
using AIExtensionsCenter.Domain.Entities;
using Ardalis.GuardClauses;

namespace AIExtensionsCenter.Application.Licenses.Queries.GetLicenseByExtensionId;

public record GetLicenseByExtensionIdQuery(Guid Id) : IRequest<List<LicenseVM>>
{
}

public class GetLicenseByExtensionIdQueryValidator : AbstractValidator<GetLicenseByExtensionIdQuery>
{
    public GetLicenseByExtensionIdQueryValidator()
    {
    }
}

public class GetLicenseByExtensionIdQueryHandler : IRequestHandler<GetLicenseByExtensionIdQuery, List<LicenseVM>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetLicenseByExtensionIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<LicenseVM>> Handle(GetLicenseByExtensionIdQuery request, CancellationToken cancellationToken)
    {
        List<License> licenses = await _context.Licenses
            .Where(x => x.ExtensionId == request.Id)
            .ToListAsync(cancellationToken);
        Guard.Against.NotFound(request.Id, licenses);

        return _mapper.Map<List<LicenseVM>>(licenses);
    }
}
