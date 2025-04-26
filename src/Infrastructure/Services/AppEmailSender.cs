using AIExtensionsCenter.Application.Common.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace AIExtensionsCenter.Infrastructure.Services;

public class AppEmailSender : IAppEmailSender
{
    private readonly IConfiguration _configuration;

    public AppEmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var senderEmail = _configuration["EmailSettings:SenderEmail"];
        var smtpServer = _configuration["EmailSettings:SmtpServer"];
        var smtpPort = _configuration["EmailSettings:SmtpPort"];
        var userName = _configuration["EmailSettings:Username"];
        var password = _configuration["EmailSettings:Password"];

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("AI Extensions Center", senderEmail));
        email.To.Add(new MailboxAddress(to, to));
        email.Subject = subject;

        email.Body = new TextPart("html") { Text = body };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(smtpServer, int.Parse(smtpPort ?? "587"), MailKit.Security.SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(userName, password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
