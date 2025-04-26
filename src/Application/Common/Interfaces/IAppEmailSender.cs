namespace AIExtensionsCenter.Application.Common.Interfaces
{
    public interface IAppEmailSender
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
