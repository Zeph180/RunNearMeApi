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

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.ParentComment)
            .WithMany(c => c.Replies)
            .HasForeignKey(c => c.ParentCommentId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Post>()
            .HasMany(p => p.Comments)
            .WithOne(c => c.Post)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);
        
        
        modelBuilder.Entity<Post>()
            .HasOne(p => p.Poster)
            .WithMany() // or WithMany(profile => profile.Posts) if you have navigation property
            .HasForeignKey(p => p.RunnerId)
            .OnDelete(DeleteBehavior.NoAction);
        
        // Runner configuration
        modelBuilder.Entity<Runner>()
            .HasIndex(r => r.Email)
            .IsUnique();

        // Profile to Notifications relationship
        modelBuilder.Entity<Profile>()
            .HasMany(x => x.Notifications)
            .WithOne(x => x.Profile)
            .IsRequired();
    
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
        
        // Run to Profile relationship
        modelBuilder.Entity<Run>(entity =>
        {
            entity.HasKey(r => r.RunId);
            
            // Configure spatial properties with SRID 4326 (WGS84 - GPS coordinates)
            entity.Property(r => r.Route)
                .HasColumnType("geometry");
                
            entity.Property(r => r.RouteBounds)
                .HasColumnType("geometry");
                
            entity.Property(r => r.StartPoint)
                .HasColumnType("geometry");
                
            entity.Property(r => r.EndPoint)
                .HasColumnType("geometry");

            // Relationships
            entity.HasOne(r => r.Profile)
                .WithMany()
                .HasForeignKey(r => r.RunnerId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(r => r.RoutePoints)
                .WithOne(rp => rp.Run)
                .HasForeignKey(rp => rp.RunId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes for performance
            entity.HasIndex(r => r.RunnerId);
            entity.HasIndex(r => r.StartTime);
            entity.HasIndex(r => r.CreatedAt);
        });


        // Configure decimal precision for GPS coordinates
        modelBuilder.Entity<RunRoutePoint>(entity =>
        {
            entity.HasKey(rp => rp.RunRoutePointId);
            
            // Configure spatial property
            entity.Property(rp => rp.Location)
                .HasColumnType("geometry")
                .IsRequired();

            // Ignore computed properties
            entity.Ignore(rp => rp.Latitude);
            entity.Ignore(rp => rp.Longitude);

            // Relationship
            entity.HasOne(rp => rp.Run)
                .WithMany(r => r.RoutePoints)
                .HasForeignKey(rp => rp.RunId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            entity.HasIndex(rp => rp.RunId);
            entity.HasIndex(rp => rp.SequenceNumber);
            entity.HasIndex(rp => rp.Timestamp);
        });
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
    public DbSet<DeviceToken> DeviceTokens { get; set; }
}