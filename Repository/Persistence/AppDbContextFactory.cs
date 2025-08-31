using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Repository.Persistence;

public class AppDbContextFactory :IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        var connectionString = "Server=localhost;Database=RunNearMe;User Id=sa;Password=Kampala123456;TrustServerCertificate=True";
        optionsBuilder.UseSqlServer(connectionString, x => x.UseNetTopologySuite());
        return new AppDbContext(optionsBuilder.Options);
    }
}