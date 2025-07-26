using Application.Interfaces;
using Application.Interfaces.Dtos;
using Application.Middlewares.ErrorHandling;
using AutoMapper;
using Domain.Entities;
using Domain.Models;
using Domain.Models.Request.Account;
using Domain.Models.Response;
using Microsoft.EntityFrameworkCore;
using Repository.Persistence;

namespace Repository.Repositories;

public class RunnerRepository : IRunner
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public RunnerRepository(AppDbContext dbContext, IMapper mapper)
    {
        var runner = 
        this._dbContext = dbContext;
        this._mapper = mapper;
    }

    public async Task<Runner> CreateRunner<RunnerDto, Runner>(RunnerDto runnerDto)
    {
        var runner = _mapper.Map<RunnerDto, Runner>(runnerDto);
        var resp = await _dbContext.AddAsync(runner);
        await _dbContext.SaveChangesAsync();
        return runner;
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
        return _mapper.Map<Runner, CreateAccountResponse>(runner);
    }
}