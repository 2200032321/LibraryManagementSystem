namespace WebApp.Models
{
    public class LoginResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public LoginData Data { get; set; }
    }

    public class LoginData
    {
        public string Token { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public int UserId { get; set; }

        public DateTime ExpiresAt { get; set; }
    }
}