using Domain.Entities;

namespace Domain.Models.Response;

public class LoginResponse
{
    public string? Token { get; set; }
    public CreateAccountResponse? Account { get; set; }
    public Profile? Profile { get; set; }
}