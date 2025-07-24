using Application.Exensions;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repository.Mapping;
using Repository.Persistence;
using Repository.Repositories;

namespace RunNearMe;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();
        
        //Register DB Context with DI
        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")));

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddAutoMapper(_ => { }, typeof(MapperService).Assembly);
        builder.Services.AddControllers();
        builder.Services.AddLogging();
        builder.Services.AddScoped<IRunner, RunnerRepository>();

        var app = builder.Build();

        app.UseGlobalExceptionHandling();

        // Configure the HTTP request pipeline.e
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}