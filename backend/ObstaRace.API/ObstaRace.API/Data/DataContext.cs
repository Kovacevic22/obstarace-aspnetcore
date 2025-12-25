using Microsoft.EntityFrameworkCore;
using ObstaRace.API.Models;

namespace ObstaRace.API.Data;

public class DataContext : DbContext    
{
    public DataContext(DbContextOptions<DataContext> options) : base( options)
    {
        
    }
    public DbSet<Obstacle> Obstacles { get; set; }
    public DbSet<Race> Races { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Registration> Registrations { get; set; }
    public DbSet<RaceObstacle> RaceObstacles { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RaceObstacle>().HasKey(pc => new { pc.RaceId, pc.ObstacleId });
        modelBuilder.Entity<RaceObstacle>().HasOne(p => p.Race).WithMany(p => p.RaceObstacles).HasForeignKey(po => po.RaceId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<RaceObstacle>().HasOne(p => p.Obstacle).WithMany(p => p.RaceObstacles).HasForeignKey(po => po.ObstacleId).OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Registration>().HasOne(p => p.Race).WithMany(p => p.Registrations).HasForeignKey(po => po.RaceId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Registration>().HasOne(p => p.User).WithMany(p => p.Registrations).HasForeignKey(po => po.UserId).OnDelete(DeleteBehavior.Restrict);
        
    }
}