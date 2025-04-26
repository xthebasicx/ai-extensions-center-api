using AIExtensionsCenter.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace AIExtensionsCenter.Web.Services;

public class IdentityEmailSenderAdapter : IEmailSender
{
    private readonly IAppEmailSender _emailSender;

    public IdentityEmailSenderAdapter(IAppEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        return _emailSender.SendEmailAsync(email, subject, htmlMessage);
    }
}
