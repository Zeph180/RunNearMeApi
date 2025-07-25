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
}