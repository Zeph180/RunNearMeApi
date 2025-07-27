using Application.Interfaces;
using Application.Middlewares.ErrorHandling;
using Application.Models.Response.People;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository.Persistence;

namespace Repository.Repositories;

public class PeopleService : IPeople
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;

    public PeopleService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration, IEmailService emailService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configuration = configuration;
        _emailService = emailService;
    }

    public async Task<List<Person>> GetPeople(Guid runnerId)
    {
        if (!_dbContext.Profiles.Any(r => r.RunnerId == runnerId))
        {
            throw new BusinessException(
                "Either runner not found or profile incomplete.",
                "RUNNER_NOT_FOUND",
                404);
        }

        var people = await _dbContext.Profiles
            .Where(p => p.RunnerId != runnerId)
            .Select(p => new Person
            {
                RunnerId = p.RunnerId,
                NickName = p.NickName
            }).ToListAsync();
        return people;
    }
}