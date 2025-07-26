using Application.Exensions;
using Application.Filters;
using Application.Interfaces;
using Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
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

        builder.Logging.AddSeq();
        // Add services to the container.
        builder.Services.AddAuthorization();
        
        //Register DB Context with DI
        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")));

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddAutoMapper(_ => { }, typeof(MapperService).Assembly);
        builder.Services.AddScoped<ValidationFilter>();
        builder.Services.AddControllers(
            options => options.Filters.Add<ValidationFilter>());
        builder.Services.AddLogging();
        builder.Services.AddScoped<IRunner, RunnerRepository>();
        
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddFluentValidationClientsideAdapters();
        builder.Services.AddValidatorsFromAssemblyContaining<RunnerValidation>();

        //Disable default model validation to use only FluentValidation
        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        var app = builder.Build();
        
        // Configure the HTTP request pipeline.e
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseGlobalExceptionHandling();

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}