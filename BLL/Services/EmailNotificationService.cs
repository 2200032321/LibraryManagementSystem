using LibraryManagementSystem.DAL.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using LibraryManagementSystem.DOL.Entities;
using LibraryManagementSystem.BLL.Services.Interfaces;
public class EmailNotificationService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public EmailNotificationService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                // 🔴 Call your logic here
                await SendOverdueEmails(uow, emailService);
                await SendDueTomorrowEmails(uow, emailService);
            }

            // ⏰ run every 24 hours (or change to 1 hour for testing)
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }

    private async Task SendOverdueEmails(IUnitOfWork uow, IEmailService email)
    {
        var overdue = await uow.Transactions.Query()
            .Include(t => t.User)
            .Include(t => t.Book)
            .Where(t => t.Status == TransactionStatus.Issued
                     && t.DueDate < DateTime.UtcNow)
            .ToListAsync();

        foreach (var t in overdue)
        {
            await email.SendEmailAsync(
                t.User.Email,
                "Book Overdue",
                $"Your book '{t.Book.Title}' is overdue. Please return it."
            );
        }
    }

    private async Task SendDueTomorrowEmails(IUnitOfWork uow, IEmailService email)
    {
        var tomorrow = DateTime.UtcNow.AddDays(1);

        var due = await uow.Transactions.Query()
            .Include(t => t.User)
            .Include(t => t.Book)
            .Where(t => t.Status == TransactionStatus.Issued
                     && t.DueDate.Date == tomorrow.Date)
            .ToListAsync();

        foreach (var t in due)
        {
            await email.SendEmailAsync(
                t.User.Email,
                "Book Due Tomorrow",
                $"Your book '{t.Book.Title}' is due tomorrow."
            );
        }
    }
}