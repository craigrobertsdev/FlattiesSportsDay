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

        modelBuilder.Entity<Room>().HasMany<House>()
            .WithMany();
        
        modelBuilder.Entity<House>()
            .HasKey(x => x.Id);
        
        modelBuilder.Entity<House>()
            .HasMany<Event>()
            .WithMany();
        
        modelBuilder.Entity<Event>()
            .HasKey(x => x.Id);
    }
    
}
//
// public class DbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
// {
//     public ApplicationDbContext CreateDbContext(string[] args)
//     {
//         var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
//
//         return new ApplicationDbContext(optionsBuilder.Options);
//     }
// }