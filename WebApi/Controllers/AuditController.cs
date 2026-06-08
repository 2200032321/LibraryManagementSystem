using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.DOL.Entities;
using LibraryManagementSystem.DAL.Context;

[ApiController]
[Route("api/audit-logs")]
public class AuditController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuditController(AppDbContext context)
    {
        _context = context;
    }

    // ---------------- GET WITH FILTERS ----------------
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? userId,
        [FromQuery] string? action,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate)
    {
        var query = _context.AuditLogs.AsQueryable();

        // filter by user
        if (userId.HasValue)
            query = query.Where(x => x.UserId == userId);

        // filter by action
        if (!string.IsNullOrEmpty(action))
            query = query.Where(x => x.Action == action);

        // filter by date range
        if (fromDate.HasValue)
            query = query.Where(x => x.CreatedAt >= fromDate);

        if (toDate.HasValue)
            query = query.Where(x => x.CreatedAt <= toDate);

        var result = await query
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return Ok(result);
    }
}