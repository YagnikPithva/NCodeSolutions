using EmployeeAPI.Models;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPI.Data;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
        Database.EnsureCreated();
    }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Organization>(o =>
        {
            o.Property(x => x.Name).HasMaxLength(100).IsRequired();
            o.HasIndex(x => x.Name).IsUnique();
        });

        b.Entity<Employee>(e =>
        {
            e.Property(x => x.FullName).HasMaxLength(100).IsRequired();
            e.Property(x => x.Age).IsRequired();
            e.Property(x => x.MobileNumber).HasMaxLength(15).IsRequired();
            e.Property(x => x.Address).HasMaxLength(100).IsRequired();
            e.Property(x => x.Designation).HasMaxLength(50).IsRequired();
            e.Property(x => x.Email).HasMaxLength(150).IsRequired();
            e.Property(x => x.Salary).HasPrecision(18, 2);
            e.HasIndex(x => x.Email).IsUnique();
            e.HasOne(x => x.Organization)
             .WithMany(o => o.Employees)
             .HasForeignKey(x => x.OrganizationId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<User>(u =>
        {
            u.Property(x => x.Username).HasMaxLength(50).IsRequired();
            u.Property(x => x.Role).HasMaxLength(20).IsRequired();
            u.HasIndex(x => x.Username).IsUnique();
            u.HasIndex(x => x.EmployeeId).IsUnique();       // only one login per employee
            u.HasIndex(x => x.OrganizationId).IsUnique();   // only one login per organization

            // Deleting an employee also deletes their login - cascade deletion
            u.HasOne(x => x.Employee).WithMany()
             .HasForeignKey(x => x.EmployeeId)
             .OnDelete(DeleteBehavior.Cascade);

            // Restrict here avoids SQL Server's "multiple cascade paths" error - no cascade deletion
            u.HasOne(x => x.Organization).WithMany()
             .HasForeignKey(x => x.OrganizationId)
             .OnDelete(DeleteBehavior.Restrict);
        });



    }
}