using Application.Interfaces;
using Application.Models.Request.Challenges;
using Application.Wrappers;
using Azure.Core;
using Domain.Entities;
using Domain.Models.Request.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RunNearMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChallengesController : ControllerBase
    {
        private readonly IChallenge _challenger;

        public ChallengesController(IChallenge challenger)
        {
            _challenger = challenger;
        }

        /// <summary>
        /// This returns challenge by id
        /// </summary>
        /// <param name="challengeId"></param>
        /// <returns></returns>
        [HttpGet("{challengeId:guid}")]
        public async Task<ActionResult> GetChallengeById(Guid challengeId)
        {
            var response = await _challenger.GetChallengeById(challengeId);

            return Ok(ApiResponse<object>.SuccessResponse(response));
        }
        /// <summary>
        /// This endpoint is for runners to join challenges
        /// </summary>
        /// <param name="challengeId"></param>
        /// <param name="runnerId"></param>
        /// <returns></returns>
        [HttpPost("challengeId/{challengeId:guid}/runnerId/{runnerId:guid}")]
        public async Task<IActionResult> JoinChallenge(Guid challengeId, Guid runnerId)
        {
            var response = await _challenger.JoinChallenge(challengeId, runnerId);
            return Ok(ApiResponse<object>.SuccessResponse(response));
        }

        /// <summary>
        /// This is responsible for creating a new challenge
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateChallenge(CreateChallengeRequest request)
        {
            var response = await _challenger.CreateChallenge(request);
            return Ok(ApiResponse<object>.SuccessResponse(response));
        }
    }
}
