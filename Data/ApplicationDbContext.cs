using Microsoft.EntityFrameworkCore;
using SportsDayScoring.Models;

namespace SportsDayScoring.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options) {
    public DbSet<Class> Rooms { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
    }
}
