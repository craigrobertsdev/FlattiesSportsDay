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
            .HasMany(r => r.Houses)
            .WithMany();

        modelBuilder.Entity<Room>()
            .HasMany<Event>()
            .WithOne()
            .HasForeignKey(e => e.RoomId);
        
        modelBuilder.Entity<House>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<Event>()
            .HasKey(x => new {x.RoomId, x.HouseId, x.Name});

        modelBuilder.Entity<Event>()
            .HasOne<Room>()
            .WithMany()
            .HasForeignKey(e => e.RoomId);

        modelBuilder.Entity<Event>()
            .HasOne<House>()
            .WithMany(h => h.Events);
    }
}