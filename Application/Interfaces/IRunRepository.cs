using Application.Interfaces.Dtos.Run;
using Application.Models.Request.Run;
using Application.Models.Response.Run;
using Application.Validators.RequestValidations;
using Run = Domain.Entities.Run;

namespace Application.Interfaces;

public interface IRunRepository
{
    Task<CreateRunResponse> StartRun(Guid userId);
    Task<CompleteRunResponse> CompleteRun(RunDto request);
    Task<UpdateRunResponse> AddRoutePoint(AddRoutePointRequest request);
    Task<List<Run>> GetRunsNearLocationAsync(NearByRunRequest request);
}