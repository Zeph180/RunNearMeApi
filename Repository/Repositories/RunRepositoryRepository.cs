using Application.Errors;
using Application.Interfaces;
using Application.Interfaces.Dtos.Run;
using Application.Middlewares.ErrorHandling;
using Application.Models.Request.Run;
using Application.Models.Response.Run;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Repository.Persistence;

namespace Repository.Repositories;

public class RunRepositoryRepository : IRunRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IPeopleHelper _peopleHelper;
    private readonly IMapper _mapper;
    private readonly GeometryFactory _geometryFactory;

    public RunRepositoryRepository(AppDbContext dbContext, IPeopleHelper peopleHelper,  IMapper mapper,  GeometryFactory geometryFactory)
    {
        _dbContext = dbContext;
        _peopleHelper = peopleHelper;
        _mapper = mapper;
        _geometryFactory = geometryFactory;
    }

    public async Task<CreateRunResponse> StartRun(Guid runnerId)
    {
        await _peopleHelper.GetValidProfileAsync(runnerId, ErrorCodes.PersonNotFound, ErrorMessages.PersonNotFound);
        await IncompleteRuns(runnerId);
        
        var run = new Run
        {
            RunnerId = runnerId,
            StartTime = DateTime.UtcNow
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

    public async Task<CompleteRunResponse> CompleteRun(RunDto request)
    {
        var profile = await _peopleHelper.GetValidProfileAsync(request.RunnerId, ErrorCodes.PersonNotFound, ErrorMessages.PersonNotFound);
        _dbContext.Attach(profile);
        var run = await IncompleteRuns(request.RunnerId, true);
        _dbContext.Attach(run);
        run.EndTime = request.EndTime;
        run.DistanceInMeters =  request.DistanceInMeters;
        run.DurationInSeconds = request.DurationInSeconds;
        run.AveragePace = request.AveragePace;
        run.MaxPace = request.MaxPace;
        run.CaloriesBurned = GetCaloriesBurned(request.DistanceInMeters, profile.Weight, request.DurationInSeconds);
        run.AverageHeartRate = request.AverageHeartRate;
        run.UpdatedAt = DateTime.UtcNow;
        run.Profile = profile;

        if (request.RoutePoints?.Any() == true)
        {
            await AddRouteDataToRun(run, request.RoutePoints);
        }
        
        await _dbContext.SaveChangesAsync();

        return new CompleteRunResponse
        {
            RunId = run.RunId,
            Success = true,
            Message = "Run completed successfully"
        };
    }
    
    //Update run with live tracking (for real-time updates)
    public async Task<UpdateRunResponse> AddRoutePoint(AddRoutePointRequest request)
    {
        var run = await IncompleteRuns(request.RunnerId, true);

        var routePoint = new RunRoutePoint
        {
            RunId = request.RunId,
            Location = _geometryFactory.CreatePoint(new Coordinate(
                (double)request.Longitude, 
                (double)request.Latitude)),
            Altitude = request.Altitude,
            Timestamp = request.Timestamp ?? DateTime.UtcNow,
            SequenceNumber = await GetNextSequenceNumber(request.RunId)
        };

        _dbContext.RunRoutePoints.Add(routePoint);
        await _dbContext.SaveChangesAsync();

        return new UpdateRunResponse
        {
            Success = true,
            PointId = routePoint.RunRoutePointId
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
    
    public async Task<List<Run>>  GetRunsNearLocationAsync(NearByRunRequest request)
    {
        var searchPoint = _geometryFactory.CreatePoint(new Coordinate(request.Longitude , request.Latitude));
        
        return await _dbContext.Runs
            .Where(r => r.Route != null && 
                        r.EndTime.HasValue && 
                        r.Route.IsWithinDistance(searchPoint, request.RadiusInMeters))
            .Include(r => r.Profile)
            .OrderByDescending(r => r.StartTime)
            .ToListAsync();
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
    
     private async Task AddRouteDataToRun(Run run, List<RoutePointDto> routePoints)
    {
        // Clear existing route points if any
        if (run.RoutePoints.Any())
        {
            _dbContext.RunRoutePoints.RemoveRange(run.RoutePoints);
        }

        // Create route points
        var coordinates = new List<Coordinate>();
        var runRoutePoints = new List<RunRoutePoint>();

        for (int i = 0; i < routePoints.Count; i++)
        {
            var routePointDto = routePoints[i];
            var coordinate = new Coordinate((double)routePointDto.Longitude, (double)routePointDto.Latitude);
            coordinates.Add(coordinate);

            var runRoutePoint = new RunRoutePoint
            {
                RunId = run.RunId,
                Location = _geometryFactory.CreatePoint(coordinate),
                Altitude = routePointDto.Altitude,
                Timestamp = routePointDto.Timestamp,
                SequenceNumber = i,
                Run = run,
            };

            runRoutePoints.Add(runRoutePoint);
        }

        // Create spatial geometries if we have enough points
        // This is the entire run map
        if (coordinates.Count > 0)
        {
            run.StartPoint = _geometryFactory.CreatePoint(coordinates.First());
            
            if (coordinates.Count > 1)
            {
                run.EndPoint = _geometryFactory.CreatePoint(coordinates.Last());
                run.Route = _geometryFactory.CreateLineString(coordinates.ToArray());
                
                // Create bounding box
                var envelope = run.Route.EnvelopeInternal;
                var boundingCoords = new[]
                {
                    new Coordinate(envelope.MinX, envelope.MinY),
                    new Coordinate(envelope.MaxX, envelope.MinY),
                    new Coordinate(envelope.MaxX, envelope.MaxY),
                    new Coordinate(envelope.MinX, envelope.MaxY),
                    new Coordinate(envelope.MinX, envelope.MinY)
                };
                run.RouteBounds = _geometryFactory.CreatePolygon(boundingCoords);
            }
        }

        _dbContext.RunRoutePoints.AddRange(runRoutePoints);
    }

    private async Task<int> GetNextSequenceNumber(Guid runId)
    {
        var maxSequence = await _dbContext.RunRoutePoints
            .Where(rp => rp.RunId == runId)
            .MaxAsync(rp => (int?)rp.SequenceNumber) ?? 0;
        
        return maxSequence + 1;
    }

    private double GetCaloriesBurned(double distanceInMeters, double weightKg, double durationSeconds)
    {
        var paceInMetersPerSecond = GetPaceInMetersPerSecond(distanceInMeters, durationSeconds);
        var speedKmh = paceInMetersPerSecond * 3.6;
        var met = speedKmh switch
        {
            <= 6 => 6.0,
            <= 8 => 8.3,
            <= 10 => 10.0,
            <= 12 => 11.5,
            <= 14 => 12.5,
            _ => 13.5
        };
        var durationHours = durationSeconds / 3600.0;
        return met * weightKg * durationHours;
    }
    
    private double GetPaceInMetersPerSecond(double distanceInMeters, double durationInSeconds)
    {
        return distanceInMeters / durationInSeconds;
    }
}