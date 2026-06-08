public class EmailLog
{
    public int Id { get; set; }

    public string ToEmail { get; set; }
    public string Subject { get; set; }

    public string Body { get; set; }

    public bool IsSent { get; set; }

    public string? ErrorMessage { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? SentAt { get; set; }
}