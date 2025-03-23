using AIExtensionsCenter.Application.Common.Interfaces;

namespace AIExtensionsCenter.Application.Licenses.Commands.SendLicenseEmail;

public record SendLicenseEmailCommand : IRequest
{
    public string Email { get; init; } = null!;
    public string LicenseKey { get; init; } = null!;
}

public class SendLicenseEmailCommandValidator : AbstractValidator<SendLicenseEmailCommand>
{
    public SendLicenseEmailCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.LicenseKey)
            .NotEmpty();
    }
}

public class SendLicenseEmailCommandHandler : IRequestHandler<SendLicenseEmailCommand>
{
    private readonly IEmailSender _sender;

    public SendLicenseEmailCommandHandler(IEmailSender sender)
    {
        _sender = sender;
    }

    public async Task Handle(SendLicenseEmailCommand request, CancellationToken cancellationToken)
    {
        var subject = "AI Extensions Center";
        var body = $"<p>Your license key is: <strong>{request.LicenseKey}</strong></p>";

        await _sender.SendEmailAsync(request.Email, subject, body);
    }
}
