using System.Text;
using Application.Exensions;
using Application.Filters;
using Application.Interfaces;
using Application.Validators;
using Domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
       // builder.Services.AddAuthentication().AddJwtBearer();
        builder.Services.AddAuthorization();
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        //Register DB Context with DI
        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")));
        
        //Add JWT Authentication
        var jwtSettings = builder.Configuration.GetSection("jwtSettings");
        var key = Encoding.UTF8.GetBytes(jwtSettings["secretKey"]);
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
              ValidateIssuer = true,
              ValidateAudience = true,
              ValidateLifetime = true,
              ValidateIssuerSigningKey = true,
              ValidIssuer = jwtSettings["issuer"],
              ValidAudience = jwtSettings["audience"],
              IssuerSigningKey = new SymmetricSecurityKey(key),
              ClockSkew = TimeSpan.Zero
            };
        });
        
        builder.Services.AddAutoMapper(_ => { }, typeof(MapperService).Assembly);
        builder.Services.AddScoped<ValidationFilter>();
        builder.Services.AddControllers(
            options => options.Filters.Add<ValidationFilter>());
        builder.Services.AddLogging();
        builder.Services.AddScoped<IRunner, RunnerRepository>();
        builder.Services.AddScoped<IAuthentication, AuthenticationRepository>();
        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddScoped<IPeople, PeopleService>();
        
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
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}