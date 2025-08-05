using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RunNearMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChallengesController : ControllerBase
    {
        // GET: api/groups
        [HttpGet]
        public IActionResult GetGroups()
        {
            // This is a placeholder for the actual implementation.
            // To retrieve groups from a database here
            return Ok(new { Message = "List of groups" });
        }
        // GET: api/groups/{id}
        [HttpGet("{id}")]
        public IActionResult GetGroupById(int id)
        {
            // This is a placeholder for the actual implementation.
            // To retrieve specific group by its ID from a database
            return Ok(new { Message = $"Details of group with ID {id}" });
        }
    }
}
