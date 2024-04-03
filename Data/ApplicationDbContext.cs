using Microsoft.EntityFrameworkCore;
using SportsDayScoring.Models;

namespace SportsDayScoring.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options) {
    public DbSet<Class> Rooms { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Class>()
            .HasKey()

        modelBuilder.Entity<Class>()
            .OwnsMany(c => c.Houses, hb => { 
                hb.HasKey(h => h.Id);

                hb.OwnsMany(h => h.Events, eb => {
                    eb.HasKey(e => e.Id);
                });
            });

        modelBuilder.Entity<Class>().HasData(Seed.Rooms);
    }
}
