using Application.Models.Request.People;
using Application.Models.Response;
using Application.Models.Response.People;

namespace Application.Interfaces;

public interface IPeople
{
    Task<List<Person>> GetPeople(Guid runnerId);
    Task<GetPersonResponse> GetPerson(GetPersonRequest request);
    Task<FriendRequestResponse> SendFriendRequest(GetPersonRequest request);
    Task<FriendRequestResponse> GetFriendRequest (GetFriendRequestRequest request);
    Task<List<FriendRequestResponse>> GetFriendRequests (Guid request);
    Task<FriendRequestResponse> UpdateFriendRequest(UpdateFriendShip request);
}