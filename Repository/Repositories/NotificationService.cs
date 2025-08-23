using System.Net;
using System.Net.Mail;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Repository.Persistence;

namespace Repository.Repositories;

public class NotificationService: INotificationService
{
    private readonly AppDbContext _dbContext;
    private readonly IConfiguration _config;

    public NotificationService(AppDbContext dbContext,  IConfiguration configuration)
    {
        _dbContext = dbContext;
        _config = configuration;
    }
    
    public async Task SendEmailAsync(string to, string subject, string htmlBody)
    {
        var smtpClient = new SmtpClient(_config["EmailSettings:SmtpServer"])
        {
            Port = int.Parse(_config["EmailSettings:Port"]!),
            Credentials = new NetworkCredential(
                _config["EmailSettings:Username"],
                _config["EmailSettings:Password"]
            ),
            EnableSsl = true
        };

        var mail = new MailMessage
        {
            From = new MailAddress(_config["EmailSettings:FromEmail"]!, _config["EmailSettings:FromName"]),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };

        mail.To.Add(new MailAddress(to));

        await smtpClient.SendMailAsync(mail);
    }

    public async Task SendPushNotificationAsync(string to, string subject, string message)
    {
        throw new NotImplementedException();
    }

    public async Task SendSmsAsync(string number, string message)
    {
        throw new NotImplementedException();
    }

}