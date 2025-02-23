using AIExtensionsCenter.Application.Common.Exceptions;
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
    private readonly IUser _user;

    public DeleteAPIKeyCommandHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task Handle(DeleteAPIKeyCommand request, CancellationToken cancellationToken)
    {
        APIKey? apikey = await _context.APIKeys.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        Guard.Against.NotFound(request.Id, apikey);

        if (apikey.UserId != _user.Id) throw new ForbiddenAccessException();

        _context.APIKeys.Remove(apikey);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
