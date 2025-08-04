using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Application.Middlewares.ErrorHandling;
using Application.Models.Request.Authentication;
using AutoMapper;
using Domain.Entities;
using Domain.Models.Request.Account;
using Domain.Models.Request.Authentication;
using Domain.Models.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.Persistence;
using Profile = Domain.Entities.Profile;

namespace Repository.Repositories;

public class AuthenticationRepository : IAuthentication
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly INotificationService _notification;
    
    public AuthenticationRepository(AppDbContext dbContext, IMapper mapper, IConfiguration configuration, INotificationService notificationService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configuration = configuration;
        _notification =  notificationService;
    }
    
    public async Task<LoginResponse> CompleteProfile(CompleteProfileReq profileReq)
    {
        var user = await _dbContext.Runners
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.RunnerId == profileReq.RunnerId);
        
        if (user == null)
        {
            throw new BusinessException(
                "User not found",
                "USER_NOT_FOUND",
                404
            );
        }
        
        var profile = _mapper.Map<CompleteProfileReq, Profile>(profileReq);
        var resp = await _dbContext.AddAsync(profile);
        await _dbContext.SaveChangesAsync();
        return new LoginResponse
        {
            Account = _mapper.Map<Runner, CreateAccountResponse>(user),
            Profile = profile
        };
    }
    
    public async Task<CreateAccountResponse> CreateAccount(AccountCreateRequest request)
    {
        if (await _dbContext.Runners.AnyAsync(r => r.Email == request.Email))
        {
            throw new BusinessException(
                "An account with this email already exists.",
                "DUPLICATE_EMAIL",
                409);
        }
        
        var runner = _mapper.Map<AccountCreateRequest, Runner>(request);
        var resp = await _dbContext.AddAsync(runner);
        await _dbContext.SaveChangesAsync();
        await GenerateEmailConfirmationToken(runner.Email);
        return _mapper.Map<Runner, CreateAccountResponse>(runner);
    }

    public async Task<LoginResponse> Login(LoginRequest request)
    {
        var user = _dbContext.Runners.SingleOrDefault(r => r.Email == request.Email && r.Password == request.Password);;

        if (user == null)
        {
            throw new BusinessException(
                "Either email or password is invalid",
                "INVALID_CREDENTIALS",
                404
            );
        }
        
        var token = await GenerateJwtToken(user);
        var profile = await _dbContext.Profiles.AsNoTracking().FirstOrDefaultAsync(u => u.RunnerId == user.RunnerId);

        return new LoginResponse
        {
            Token = token,
            Account = profile != null ? _mapper.Map<Runner, CreateAccountResponse>(user) : null,
            Profile = profile
        };
    }

    private async Task<string> GenerateJwtToken(Runner runner)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, runner.RunnerId.ToString()),
            new Claim(ClaimTypes.Email, runner.Email),
            new Claim(ClaimTypes.Name, runner.Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        
        //TODO
        //CONSIDER ADDING ROLES

        var token = new JwtSecurityToken(
            issuer:  jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(Convert.ToDouble(jwtSettings["ExpirationInDays"])),
            signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task ConfirmEmail(string email)
    {
        var runner = await _dbContext.Runners.FirstOrDefaultAsync(r => r.Email == email);
        
        if (runner == null)
        {
            throw new BusinessException(
                "Email not found",
                "EMAIL_NOT_FOUND",
                404
            );
        }
        
        runner.EmailConfirmed = false;
        runner.TokenConfirmedAt = DateTime.Now;
        _dbContext.Runners.Update(runner);
        await _dbContext.SaveChangesAsync();
    }

    private async Task GenerateEmailConfirmationToken(string email)
    {
        var runner = await _dbContext.Runners.FirstOrDefaultAsync(r => r.Email == email);
        
        if (runner == null)
        {
            throw new BusinessException(
                "Email not found",
                "EMAIL_NOT_FOUND",
                404
            );
        }
        
        string emailToken = Guid.NewGuid().ToString().Replace("-","");
        
        runner.EmailConfirmationToken = emailToken;
        runner.EmailConfirmed = false;
        runner.TokenGeneratedAt = DateTime.Now;
        _dbContext.Runners.Update(runner);
        await _dbContext.SaveChangesAsync();
        
        //await _emailService.SendAsync(email, "Confirm your email", $"Please confirm your email by clicking this link: {emailToken}");
    }
    
    
}