using Application.Interfaces;

namespace Repository.Repositories;

public class RunnerRepository : IRunner
{
    public async Task CreateRunner<RunnerDto, Runner>()
    {
        throw new NotImplementedException();
    }
}