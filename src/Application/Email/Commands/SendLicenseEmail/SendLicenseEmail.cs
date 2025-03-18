using AIExtensionsCenter.Application.Common.Interfaces;

namespace AIExtensionsCenter.Application.Email.Commands.SendLicenseEmail;

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
    private readonly IEmailService _emailService;

    public SendLicenseEmailCommandHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Handle(SendLicenseEmailCommand request, CancellationToken cancellationToken)
    {
        var subject = "Your License Key";
        var body = $"<p>Your license key is: <strong>{request.LicenseKey}</strong></p>";

        await _emailService.SendEmailAsync(request.Email, subject, body);
    }
}
