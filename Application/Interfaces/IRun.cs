using Application.Interfaces.Dtos.Run;
using Application.Models.Request.Run;
using Application.Models.Response.Run;

namespace Application.Interfaces;

public interface IRun
{
    Task<CreateRunResponse> CreateRun(Guid userId);
    Task<RunDto> UpdateRun(RunDto request);
}