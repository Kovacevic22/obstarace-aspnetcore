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
    public DbSet<Organiser> Organisers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RaceObstacle>().HasKey(pc => new { pc.RaceId, pc.ObstacleId });
        modelBuilder.Entity<RaceObstacle>().HasOne(p => p.Race).WithMany(p => p.RaceObstacles).HasForeignKey(po => po.RaceId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<RaceObstacle>().HasOne(p => p.Obstacle).WithMany(p => p.RaceObstacles).HasForeignKey(po => po.ObstacleId).OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Registration>().HasOne(p => p.Race).WithMany(p => p.Registrations).HasForeignKey(po => po.RaceId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Registration>().HasOne(p => p.User).WithMany(p => p.Registrations).HasForeignKey(po => po.UserId).OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Registration>() .HasIndex(r => new { r.UserId, r.RaceId }).IsUnique();
        
        modelBuilder.Entity<Registration>().HasIndex(r => r.BibNumber).IsUnique();
        
        modelBuilder.Entity<Race>().Property(r => r.Date).HasColumnType("date");
        modelBuilder.Entity<Race>().Property(r => r.RegistrationDeadLine).HasColumnType("date");
        modelBuilder.Entity<Race>().HasIndex(r => r.Slug).IsUnique();
        
        modelBuilder.Entity<User>().Property(u => u.DateOfBirth).HasColumnType("date");
        
        modelBuilder.Entity<Organiser>().HasKey(o => o.UserId);
        modelBuilder.Entity<User>().HasOne(u=>u.Organiser)
            .WithOne(u=>u.User)
            .HasForeignKey<Organiser>(o=>o.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}