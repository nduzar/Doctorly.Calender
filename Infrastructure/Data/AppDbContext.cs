using Doctorly.Calendar.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doctorly.Calendar.Infrastructure.Data;

/// <summary>
/// Infrastructure Layer: Entity Framework DB Context.
/// This acts as the bridge between our Domain Entities and the SQLite Database.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<CalendarEvent> Events => Set<CalendarEvent>();
    public DbSet<Attendee> Attendees => Set<Attendee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the CalendarEvent Entity
        modelBuilder.Entity<CalendarEvent>(entity =>
        {
            entity.HasKey(e => e.Id);

            // Requirement: Concurrency check for the 'Could' requirement.
            // SQLite doesn't have a native RowVersion byte array, 
            // so we map our Version Guid to handle conflict detection.
            entity.Property(e => e.Version).IsConcurrencyToken();

            // Requirement: Define Relationship (One Event -> Many Attendees)
            entity.HasMany(e => e.Attendees)
                  .WithOne()
                  .HasForeignKey("CalendarEventId") // Shadow Property
                  .OnDelete(DeleteBehavior.Cascade); // Delete attendees if event is deleted
        });

        // Configure the Attendee Entity
        modelBuilder.Entity<Attendee>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Email).IsRequired().HasMaxLength(255);
            entity.Property(a => a.Name).IsRequired().HasMaxLength(100);
        });
    }
}