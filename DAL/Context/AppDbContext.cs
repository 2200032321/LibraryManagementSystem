using LibraryManagementSystem.DOL.Entities;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace LibraryManagementSystem.DAL.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<EmailLog> EmailLogs { get; set; }
        public DbSet<User> Users => Set<User>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<BookTransaction> BookTransactions => Set<BookTransaction>();
        public DbSet<Reservation> Reservations => Set<Reservation>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Book>()
                .HasIndex(b => b.ISBN);

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Fiction", Description = "Fiction books" },
                new Category { Id = 2, Name = "Science", Description = "Science books" },
                new Category { Id = 3, Name = "Technology", Description = "Technology books" },
                new Category { Id = 4, Name = "History", Description = "History books" },
                new Category { Id = 5, Name = "Biography", Description = "Biography books" }
            );

            // Seed Admin user (password: Admin@123)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FullName = "System Admin",
                    Email = "admin@library.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    Role = UserRole.Admin,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}