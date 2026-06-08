using LibraryManagementSystem.DAL.Context;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;
    private readonly AppDbContext _context;

    public EmailService(IOptions<EmailSettings> settings, AppDbContext context)
    {
        _settings = settings.Value;
        _context = context;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var log = new EmailLog
        {
            ToEmail = toEmail,
            Subject = subject,
            Body = body,
            IsSent = false
        };

        _context.EmailLogs.Add(log);
        await _context.SaveChangesAsync();

        try
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart("html") { Text = body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.SmtpServer, _settings.Port, false);
            await smtp.AuthenticateAsync(_settings.SenderEmail, _settings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            log.IsSent = true;
            log.SentAt = DateTime.UtcNow;
        }
        catch (Exception ex)
        {
            log.ErrorMessage = ex.Message;
        }

        await _context.SaveChangesAsync();
    }
}