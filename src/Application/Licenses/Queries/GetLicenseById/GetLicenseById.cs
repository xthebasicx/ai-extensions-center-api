using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Application.Common.Models;
using AIExtensionsCenter.Domain.Entities;
using Ardalis.GuardClauses;

namespace AIExtensionsCenter.Application.Licenses.Queries.GetLicenseById;

public record GetLicenseByIdQuery(Guid Id) : IRequest<LicenseVM>
{
}

public class GetLicenseByIdQueryValidator : AbstractValidator<GetLicenseByIdQuery>
{
    public GetLicenseByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}

public class GetLicenseByIdQueryHandler : IRequestHandler<GetLicenseByIdQuery, LicenseVM>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetLicenseByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<LicenseVM> Handle(GetLicenseByIdQuery request, CancellationToken cancellationToken)
    {
        License? license = await _context.Licenses.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        Guard.Against.NotFound(request.Id,license);

        return _mapper.Map<LicenseVM>(license);
    }
}
