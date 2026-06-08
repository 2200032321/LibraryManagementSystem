namespace LibraryManagementSystem.DOL.DTOs
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public static ApiResponse<T> Ok(T data, string msg = "Success")
            => new() { Success = true, Data = data, Message = msg };

        public static ApiResponse<T> Fail(string msg)
            => new() { Success = false, Message = msg };
    }

    public class DashboardStatsDto
    {
        public int TotalBooks { get; set; }

        public int AvailableBooks { get; set; }

        public int TotalMembers { get; set; }

        public int ActiveMembers { get; set; }

        public int IssuedBooks { get; set; }

        public int OverdueBooks { get; set; }

        public int EBooks { get; set; }

        public decimal TotalFineCollected { get; set; }

        public int TotalReservations { get; set; }

        public List<CategoryStatDto> BooksByCategory { get; set; } = new();

        public List<MonthlyStatDto> MonthlyIssues { get; set; } = new();
    }

    public class CategoryStatDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class MonthlyStatDto
    {
        public string Month { get; set; } = string.Empty;
        public int Count { get; set; }
    }
    //public class OverdueReportDto
    //{
    //    public string BookTitle { get; set; } = string.Empty;

    //    public string UserName { get; set; } = string.Empty;

    //    public int DaysOverdue { get; set; }

    //    public decimal FineAmount { get; set; }
    //}
}