using LibraryManagementSystem.DAL.Context;

public class EmailRetryService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public EmailRetryService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            var failedEmails = context.EmailLogs
                .Where(x => !x.IsSent && x.ErrorMessage != null)
                .Take(10)
                .ToList();

            foreach (var email in failedEmails)
            {
                try
                {
                    await emailService.SendEmailAsync(
                        email.ToEmail,
                        email.Subject,
                        email.Body
                    );
                }
                catch
                {
                    // ignore, will retry next cycle
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
        }
    }
}