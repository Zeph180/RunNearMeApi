using Application.Interfaces;

namespace Repository.Repositories;

public class RunnerRepository : IRunner
{
    public async Task<Runner> CreateRunner<RunnerDto, Runner>(RunnerDto runnerDto)
    {
        throw new NotImplementedException();
    }
}