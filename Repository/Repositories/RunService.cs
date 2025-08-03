using Application.Errors;
using Application.Interfaces;
using Application.Interfaces.Dtos.Run;
using Application.Middlewares.ErrorHandling;
using Application.Models.Request.Run;
using Application.Models.Response.Run;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Persistence;

namespace Repository.Repositories;

public class RunService : IRun
{
    private readonly AppDbContext _dbContext;
    private readonly IPeopleHelper _peopleHelper;
    private readonly IMapper _mapper;

    public RunService(AppDbContext dbContext, IPeopleHelper peopleHelper,  IMapper mapper)
    {
        _dbContext = dbContext;
        _peopleHelper = peopleHelper;
        _mapper = mapper;
    }

    public async Task<CreateRunResponse> CreateRun(Guid runnerId)
    {
        await _peopleHelper.GetValidProfileAsync(runnerId, ErrorCodes.PersonNotFound, ErrorMessages.PersonNotFound);
        await IncompleteRuns(runnerId);
        
        var run = new Run
        {
            RunnerId = runnerId,
        };

        _dbContext.Runs.Add(run);
        await _dbContext.SaveChangesAsync();

        var runResp = await _dbContext.Runs
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.RunId == run.RunId);

        return new CreateRunResponse
        {
            RunnerId = runResp.RunnerId,
            RunId = runResp.RunId,
        };
    }

    public async Task<RunDto> UpdateRun(RunDto request)
    {
        await _peopleHelper.GetValidProfileAsync(request.RunnerId, ErrorCodes.PersonNotFound, ErrorMessages.PersonNotFound);
        var incompleteRun = await IncompleteRuns(request.RunnerId, true);
        var run = _mapper.Map<RunDto, Run>(request);
        _dbContext.Runs.Update(run);
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<Run, RunDto>(run);
    }

    private async Task<Run> IncompleteRuns(Guid runnerId, bool track = false)
    {
        var incompleteRun = track switch
        {
            true => await _dbContext.Runs
                .FirstOrDefaultAsync(r => r.RunnerId == runnerId && r.EndTime == null),

            false => await _dbContext.Runs
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RunnerId == runnerId && r.EndTime == null)
        };
        
        if (incompleteRun != null && !track)
        {
            throw new BusinessException(ErrorMessages.IncompleteRun, ErrorCodes.ActionNotAllowed);
        }

        if (incompleteRun == null && track)
        {
            throw new BusinessException(ErrorMessages.NoIncompleteRunFound, ErrorCodes.ActionNotAllowed);
        }

        return incompleteRun;
    }
}