using Application.Models.Request.People;
using Application.Models.Response;
using Application.Models.Response.People;

namespace Application.Interfaces;

public interface IPeople
{
    Task<List<Person>> GetPeople(Guid runnerId, int pageNumber = 1, int pageSize = 10);
    Task<GetPersonResponse> GetPerson(GetPersonRequest request);
    Task<FriendRequestResponse> SendFriendRequest(GetPersonRequest request);
    Task<FriendRequestResponse> GetFriendRequest (GetFriendRequestRequest request);
    Task<FriendRequestsListResponse> GetFriendRequests (Guid request,  int pageSize = 10, int pageNumber = 1);
    Task<FriendRequestResponse> UpdateFriendRequest(UpdateFriendShip request);
}