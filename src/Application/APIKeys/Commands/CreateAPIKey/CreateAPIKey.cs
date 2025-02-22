using AIExtensionsCenter.Application.Common.Interfaces;
using AIExtensionsCenter.Domain.Entities;

namespace AIExtensionsCenter.Application.APIKeys.Commands.CreateAPIKey;

public record CreateAPIKeyCommand : IRequest<Guid>
{
}

public class CreateAPIKeyCommandValidator : AbstractValidator<CreateAPIKeyCommand>
{
    public CreateAPIKeyCommandValidator()
    {
    }
}

public class CreateAPIKeyCommandHandler : IRequestHandler<CreateAPIKeyCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public CreateAPIKeyCommandHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task<Guid> Handle(CreateAPIKeyCommand request, CancellationToken cancellationToken)
    {
        var userId = _user.Id ?? throw new UnauthorizedAccessException();
        var key = Guid.NewGuid().ToString();

        APIKey? apikey = new()
        {
            Key = key,
            UserId = userId,
        };

        _context.APIKeys.Add(apikey);
        await _context.SaveChangesAsync(cancellationToken);

        return apikey.Id;
    }
}
