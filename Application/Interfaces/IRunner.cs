using Application.Interfaces.Dtos;
using Domain.Entities;
using Domain.Models.Request.Account;

namespace Application.Interfaces;

public interface IRunner
{
    Task<T2> CreateRunner<T1,T2>(T1 runnerDto);
    Task<RunnerDto> CreateAccount(AccountCreateRequest request);
}