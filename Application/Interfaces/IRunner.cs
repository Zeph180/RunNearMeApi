namespace Application.Interfaces;

public interface IRunner
{
    Task<Runner> CreateRunner<RunnerDto, Runner>(RunnerDto runnerDto);
}