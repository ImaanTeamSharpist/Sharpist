namespace Sharpist.Server.Service.IServices;

public interface IEmailService
{
    Task<bool> SendMessageToEmailAsync(string to, string subject, string message);
}
