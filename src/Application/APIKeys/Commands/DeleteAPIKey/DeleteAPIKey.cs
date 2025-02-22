using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Domain.Entities;
using Ardalis.GuardClauses;

namespace AIExtensionsCenter.Application.APIKeys.Commands.DeleteAPIKey;

public record DeleteAPIKeyCommand(Guid Id) : IRequest
{
}

public class DeleteAPIKeyCommandValidator : AbstractValidator<DeleteAPIKeyCommand>
{
    public DeleteAPIKeyCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteAPIKeyCommandHandler : IRequestHandler<DeleteAPIKeyCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteAPIKeyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteAPIKeyCommand request, CancellationToken cancellationToken)
    {
        APIKey? apikey = await _context.APIKeys.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        Guard.Against.NotFound(request.Id, apikey);

        _context.APIKeys.Remove(apikey);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
