using Application.Interfaces;
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
      return Ok(people);
   }
}