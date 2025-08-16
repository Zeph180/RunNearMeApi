using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Only call this once, at the beginning

        // Runner configuration
        modelBuilder.Entity<Runner>()
            .HasIndex(r => r.Email)
            .IsUnique();

        // Profile to Notifications relationship
        modelBuilder.Entity<Profile>()
            .HasMany(x => x.Notifications)
            .WithOne(x => x.Profile)
            .IsRequired();
        
        // Profile to Runs relationship - REMOVE THIS since you configure it below
        // modelBuilder.Entity<Profile>()
        //     .HasMany(x => x.Runs)
        //     .WithOne(x => x.Profile)
        //     .HasForeignKey(x => x.RunnerId)
        //     .IsRequired();
    
        // Friend relationships
        modelBuilder.Entity<Friend>()
            .HasOne(f => f.RequestFromProfile)
            .WithMany()
            .HasForeignKey(f => f.RequestFrom)
            .OnDelete(DeleteBehavior.Restrict);
    
        modelBuilder.Entity<Friend>()
            .HasOne(f => f.RequestToProfile)
            .WithMany()
            .HasForeignKey(f => f.RequestTo)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Run to Profile relationship - CONSOLIDATED CONFIGURATION
        modelBuilder.Entity<Run>()
            .HasOne(r => r.Profile)
            .WithMany(p => p.Runs) // This matches your Profile.Runs property
            .HasForeignKey(r => r.RunnerId)
            .OnDelete(DeleteBehavior.NoAction) // Prevent cascade delete conflicts
            .IsRequired(); // Profile can be null

        // Configure decimal precision for GPS coordinates
        modelBuilder.Entity<RunRoutePoint>()
            .Property(rrp => rrp.Latitude)
            .HasPrecision(18, 6);
    
        modelBuilder.Entity<RunRoutePoint>()
            .Property(rrp => rrp.Longitude)
            .HasPrecision(18, 6);
    }

    public DbSet<Runner> Runners { get; set; }
    public DbSet<Run> Runs { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Friend> Friends { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Challenge> Challenges { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<RunRoutePoint> RunRoutePoints { get; set; }
}