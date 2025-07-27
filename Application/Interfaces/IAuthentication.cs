using Application.Models.Request.Authentication;
using Domain.Entities;
using Domain.Models.Request.Account;
using Domain.Models.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using LoginRequest = Domain.Models.Request.Authentication.LoginRequest;

namespace Application.Interfaces;

public interface IAuthentication
{
    Task<CreateAccountResponse> CreateAccount(AccountCreateRequest request);
    Task<LoginResponse> Login(LoginRequest request);
    Task<LoginResponse> CompleteProfile(CompleteProfileReq user);
}