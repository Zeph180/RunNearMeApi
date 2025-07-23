namespace Application.Interfaces;

public interface IRunner
{
    Task CreateRunner<RunnerDto, Runner>();
}