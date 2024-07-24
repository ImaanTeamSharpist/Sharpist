namespace Sharpist.Server.Service.IServices;

public interface IEmailService
{
    Task SendMessageToEmailAsync(string to, string subject, string message);
}
