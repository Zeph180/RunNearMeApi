using Application.Models.Request.People;
using Application.Models.Response.People;

namespace Application.Interfaces;

public interface IPeople
{
    Task<List<Person>> GetPeople(Guid runnerId);
    Task<GetPersonResponse> GetPerson(GetPersonRequest request);
    Task<FriendRequestResponse> SendFriendRequest(GetPersonRequest request);
}