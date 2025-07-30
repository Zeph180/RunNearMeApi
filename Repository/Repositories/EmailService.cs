using System.Net;
using System.Net.Mail;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repository.Persistence;

namespace Repository.Repositories;

public class EmailService : IEmailService
{
    private readonly AppDbContext _dbContext;
    private readonly IConfiguration _config;

    public EmailService(AppDbContext dbContext,  IConfiguration configuration)
    {
        _dbContext = dbContext;
        _config = configuration;
    }
    
    public async Task SendAsync(string to, string subject, string htmlBody)
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
}