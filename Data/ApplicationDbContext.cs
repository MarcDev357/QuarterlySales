using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuarterlySales.Models;

namespace QuarterlySales.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Sales> Sales { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure decimal precision for SQLite
            modelBuilder.Entity<Sales>()
                .Property(s => s.Amount)
                .HasConversion<double>(); // SQLite doesn't have decimal, use double

            // Configure relationships
            modelBuilder.Entity<Sales>()
                .HasOne(s => s.Employee)
                .WithMany()
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Manager)
                .WithMany()
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Create unique indexes for validation
            modelBuilder.Entity<Employee>()
                .HasIndex(e => new { e.Firstname, e.Lastname, e.DOB })
                .IsUnique()
                .HasDatabaseName("IX_Unique_Employee");

            modelBuilder.Entity<Sales>()
                .HasIndex(s => new { s.Quarter, s.Year, s.EmployeeId })
                .IsUnique()
                .HasDatabaseName("IX_Unique_Sales");

            // Seed data - REMOVE the HasData calls as they cause issues with SQLite
            // We'll seed in Program.cs instead
        }
    }
}