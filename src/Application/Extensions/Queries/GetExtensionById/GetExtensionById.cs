using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Application.Common.Models;
using AIExtensionsCenter.Domain.Entities;
using Ardalis.GuardClauses;

namespace AIExtensionsCenter.Application.Extensions.Queries.GetExtensionById;

public record GetExtensionByIdQuery(Guid Id) : IRequest<ExtensionVM>
{
}

public class GetExtensionByIdQueryValidator : AbstractValidator<GetExtensionByIdQuery>
{
    public GetExtensionByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}

public class GetExtensionByIdQueryHandler : IRequestHandler<GetExtensionByIdQuery, ExtensionVM>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetExtensionByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ExtensionVM> Handle(GetExtensionByIdQuery request, CancellationToken cancellationToken)
    {
        Extension? extension = await _context.Extensions.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        Guard.Against.NotFound(request.Id, extension);

        return _mapper.Map<ExtensionVM>(extension);
    }
}
