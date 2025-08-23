// Updated Program.cs
using System.Text;
using Application.Exensions;
using Application.Filters;
using Application.Interfaces;
using Application.Models.Request.PushNotification;
using Application.Validators;
using Domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository.Mapping;
using Repository.Persistence;
using Repository.Repositories;
using Repository.Repositories.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Models.Request.Cloudinary;
using Application.Services;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Features;
using Repository.Services;

namespace RunNearMe;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.AddSeq();
        builder.Services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy", options =>
            {
                options.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("https://localhost:4200");
            });
        });
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        //Register DB Context with DI
        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")));
        
        //Add JWT Authentication
        var jwtSettings = builder.Configuration.GetSection("jwtSettings");
        var googleSettings = builder.Configuration.GetSection("Auth:Google");
        var key = Encoding.UTF8.GetBytes(jwtSettings["secretKey"]);
        
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(options =>
        {
            options.Cookie.HttpOnly = false;
            options.Cookie.IsEssential = true;
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            options.IdleTimeout = TimeSpan.FromMinutes(30);
        });

        builder.Services.Configure<CookiePolicyOptions>(options =>
        {
            options.MinimumSameSitePolicy = SameSiteMode.None;
            options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.None;
            options.Secure = CookieSecurePolicy.SameAsRequest;
        });

        builder.Services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = 50 * 1024 * 1024; // 50 MB
        });

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddGoogle(options =>
            {
                var clientId = googleSettings["ClientId"];
                var clientSecret = googleSettings["ClientSecret"];

                if (string.IsNullOrEmpty(clientId))
                    throw new ArgumentNullException(nameof(clientId), "Google ClientId is required");
                
                if (string.IsNullOrEmpty(clientSecret))
                    throw new ArgumentNullException(nameof(clientSecret), "Google ClientSecret is required");


                options.ClientId = clientId;
                options.ClientSecret = clientSecret;
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                // options.CallbackPath = "/api/Authentication/google-callback";
                options.CallbackPath = "/signin-google";
                
                // Request additional scopes
                options.Scope.Clear(); // Clear default scopes first
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
        
                // Add additional scopes as needed
                options.Scope.Add("https://www.googleapis.com/auth/user.birthday.read");
                options.Scope.Add("https://www.googleapis.com/auth/user.gender.read");
                options.Scope.Add("https://www.googleapis.com/auth/user.phonenumbers.read");
                options.Scope.Add("https://www.googleapis.com/auth/user.addresses.read");
        
                // Map additional claims
                options.ClaimActions.MapJsonKey("birthday", "birthday");
                options.ClaimActions.MapJsonKey("gender", "gender");
                options.ClaimActions.MapJsonKey("phone_number", "phone_number");
                options.ClaimActions.MapJsonKey("address", "address");
                options.ClaimActions.MapJsonKey("locale", "locale");
                options.ClaimActions.MapJsonKey("email_verified", "email_verified");
                
                options.SaveTokens = true;
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
            
        builder.Services.AddAuthorization();
        
        builder.Services.AddAutoMapper(_ => { }, typeof(MapperService).Assembly);
        builder.Services.AddScoped<ValidationFilter>();
        builder.Services.AddControllers(
            options => options.Filters.Add<ValidationFilter>());
        builder.Services.AddLogging();
        
        builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
        builder.Services.AddSingleton<Cloudinary>(provider =>
        {
            var config = builder.Configuration.GetSection("Cloudinary");
            var account = new Account(
            config["CloudName"],
            config["ApiKey"],
            config["ApiSecret"]
            );
            return new Cloudinary(account);
        });
        builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
        builder.Services.AddScoped<IRunner, RunnerRepository>();
        builder.Services.AddScoped<IAuthentication, AuthenticationRepository>();
        builder.Services.AddScoped<INotificationService,NotificationService>();
        builder.Services.AddScoped<IPeople, PeopleService>();
        builder.Services.AddScoped<IRun, RunService>();
        builder.Services.AddScoped<IPeopleHelper, PeopleHelpers>();
        builder.Services.AddScoped<IChallengeRepository, ChallengeRepository>();
        builder.Services.Configure<FirebaseConfig>(
            builder.Configuration.GetSection("Firebase"));
        builder.Services.AddScoped<IPushNotificationService, FirebasePushNotificationService>();
        builder.Services.AddScoped<IDeviceTokenService, DeviceTokenService>();
        
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddFluentValidationClientsideAdapters();
        builder.Services.AddValidatorsFromAssemblyContaining<RunnerValidation>();
        
        //Disable default model validation to use only FluentValidation
        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        var app = builder.Build();
        
        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }

        app.UseGlobalExceptionHandling();

        app.UseHttpsRedirection();

        app.UseCors("CorsPolicy");

        app.UseCookiePolicy();
        
        app.UseSession();

        app.UseAuthentication();
        
        app.UseAuthorization();
        
        app.MapControllers();
        
        app.Run();
    }
}