using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Runner> Runners { get; set; }
    public DbSet<Run> Runs { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Friend> Friends { get; set; }
}