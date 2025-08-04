namespace Application.Interfaces;

public interface INotificationService
{
    Task SendEmailAsync(string to, string subject, string htmlBody);
    Task SendPushNotificationAsync(string to, string subject, string message);
    Task SendSmsAsync(string number, string message);
}