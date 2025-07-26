using Application.Interfaces.Dtos;
using Domain.Entities;
using Domain.Models.Request.Account;
using Domain.Models.Response;

namespace Application.Interfaces;

public interface IRunner
{
    Task<T2> CreateRunner<T1,T2>(T1 runnerDto);
    Task<CreateAccountResponse> CreateAccount(AccountCreateRequest request);
}