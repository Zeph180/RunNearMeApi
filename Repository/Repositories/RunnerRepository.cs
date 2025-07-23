using Application.Interfaces;
using Application.Interfaces.Dtos;
using AutoMapper;
using Domain.Entities;
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
}