using Microsoft.EntityFrameworkCore;
using SportsDayScoring.Models;

namespace SportsDayScoring.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<House> Houses { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<Room>()
            .HasMany(r => r.Events)
            .WithOne(e => e.Room);
        
        modelBuilder.Entity<House>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<Event>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<Event>()
            .OwnsMany(e => e.ScoreCards, scb =>
            {
                scb.Property<Guid>("Id");
                scb.HasKey("Id");
                scb.WithOwner().HasForeignKey("EventId");
            });
    }
}