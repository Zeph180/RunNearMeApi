using Application.Interfaces;
using Application.Models.Request.People;
using Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RunNearMe.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class People : ControllerBase
{
   private readonly IPeople _people;

   public People(IPeople people)
   {
      _people = people;
   }
   
   /// <summary>
   /// This returns all people available except the current user
   /// </summary>
   /// <param name="runnerId"></param>
   /// <returns></returns>
   [HttpGet("/get-people/{runnerId}")]
   public async Task<IActionResult> GetPeople([FromRoute]Guid runnerId)
   {
      var people = await _people.GetPeople(runnerId);
      return Ok(ApiResponse<object>.SuccessResponse(people));
   }

   /// <summary>
   /// Returns public details of a single user by their id
   /// </summary>
   /// <param name="request"></param>
   /// <returns></returns>
   [HttpPost("get-person")]
   public async Task<IActionResult> GetPerson([FromBody] GetPersonRequest request)
   {
      var person = await _people.GetPerson(request);
      return Ok(ApiResponse<object>.SuccessResponse(person));
   }

   /// <summary>
   /// Sends a friend request
   /// </summary>
   /// <param name="request"></param>
   /// <returns></returns>
   [HttpPost("request-friendship")]
   public async Task<IActionResult> SendFriendRequest([FromBody] GetPersonRequest request)
   {
      var response = await _people.SendFriendRequest(request);
      return Ok(ApiResponse<object>.SuccessResponse(response));
   }

   /// <summary>
   /// Gets a single friend request 
   /// </summary>
   /// <param name="request"></param>
   /// <returns></returns>
   [HttpPost("get-friend-request")]
   public async Task<IActionResult> GetFriendRequest([FromBody] GetFriendRequestRequest request)
   {
      var response = await _people.GetFriendRequest(request);
      return Ok(ApiResponse<object>.SuccessResponse(response));
   }

   /// <summary>
   /// Gets all friend requests of a particular user
   /// </summary>
   /// <param name="runnerId"></param>
   /// <returns></returns>
   [HttpGet("get-friend-requests/{runnerId}")]
   public async Task<IActionResult> GetFriendRequests([FromRoute] Guid runnerId)
   {
      var response = await _people.GetFriendRequests(runnerId);
      return Ok(ApiResponse<object>.SuccessResponse(response));
   }
   
   /// <summary>
   /// Used to update friend requests,
   /// 'A' = Approve, D = Declined, I = Ignored, P = Pending, C = canceled, U = Unfriend
   /// </summary>
   /// <param name="request"></param>
   /// <returns></returns>
   [HttpPost("update-friend-request")]
   public async Task<IActionResult> UpdateFriendRequest([FromBody] UpdateFriendShip request)
   {
      var response = await _people.UpdateFriendRequest(request);
      return Ok(ApiResponse<object>.SuccessResponse(response));
   }
}