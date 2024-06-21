using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestAzureDepSlots.Data;

public sealed class Repository : DbContext
{
    public DbSet<Employee> Employees { get; private set; }
    
    public Repository(DbContextOptions<Repository> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Employee).Assembly);
    }
    
    public void ApplyPendingMigrations(ILogger<DbContext> logger)
    {
        logger.LogInformation("Attempting to apply pending migrations");
        // Check for pending migrations
        var pendingMigrations = Database.GetPendingMigrations().ToList();
        if (pendingMigrations.Count == 0)
        {
            logger.LogInformation("No pending migrations to apply");
            return;
        }

        // Apply pending migrations
        logger.LogInformation("{PendingMigrationsCount} pending migrations found, applying them", pendingMigrations.Count);
        Database.Migrate();
        logger.LogInformation("Applied migrations successfully");
    }
    
}

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class EmployeeTypeConfig : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();
        builder.Property(p => p.Name).IsRequired();
    }
}