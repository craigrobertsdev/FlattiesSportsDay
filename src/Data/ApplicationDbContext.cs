using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SportsDayScoring.Models;

namespace SportsDayScoring.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Room> Rooms { get; set; }
    public DbSet<HouseEvent> HouseEvents { get; set; }
    public DbSet<House> Houses { get; set; }
    public DbSet<HouseSpirit> HouseSpirits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Room>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<Room>()
            .HasMany(r => r.HouseEvents)
            .WithOne(e => e.Room);

        modelBuilder.Entity<House>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<HouseEvent>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<HouseEvent>()
            .OwnsMany(e => e.ScoreCards, scb =>
            {
                scb.Property<Guid>("Id");
                scb.HasKey("Id");
                scb.WithOwner().HasForeignKey("EventId");
            });

        modelBuilder.Entity<HouseSpirit>()
            .HasKey(hs => hs.Id);
    }
}