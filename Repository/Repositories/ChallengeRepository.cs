using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Middlewares.ErrorHandling;
using Application.Models.Request.Challenges;
using Application.Models.Response.Challenges;
using Application.Models.Response.Run;
using AutoMapper;
using Azure.Core;
using Domain.Entities;
using Domain.Models.Request.Account;
using Domain.Models.Response;
using Microsoft.EntityFrameworkCore;
using Repository.Persistence;

namespace Repository.Repositories
{
    public  class ChallengeRepository : IChallenge
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public ChallengeRepository(AppDbContext dbContext, IMapper mapper)
        {
            var runner =
            this._dbContext = dbContext;
            this._mapper = mapper;
        }
        public async Task<CreateChallengesResponse> CreateChallenge(CreateChallengeRequest request)
        {
            // Optional: check for duplicate by Name + RunnerId, not ChallengeId, since we generate it
            if (await _dbContext.Challenges.AnyAsync(c => c.Name == request.Name && c.RunnerId == request.RunnerId))
            {
                throw new BusinessException(
                    "A challenge with this name already exists for this runner.",
                    "DUPLICATE_CHALLENGE",
                    409);
            }

            // Map request to entity
            var challenge = _mapper.Map<Challenge>(request);

            // Generate a new UUID for ChallengeId and set other properties
            challenge.ChallengeId = Guid.NewGuid();
            challenge.CreatedAt = DateTime.UtcNow;
            challenge.IsDeleted = false;

            // Save to database
            await _dbContext.Challenges.AddAsync(challenge);
            await _dbContext.SaveChangesAsync();

            // Map to response DTO
            return _mapper.Map<CreateChallengesResponse>(challenge);
        }


        public async Task<GetChallengesResp> GetChallengeById(Guid challengeId)
        {
            // Try to fetch the challenge by its Id
            var challenge = await _dbContext.Challenges
                .FirstOrDefaultAsync(c => c.ChallengeId == challengeId && !c.IsDeleted);

            if (challenge == null)
            {
                throw new BusinessException(
                    $"Challenge with Id {challengeId} not found.",
                    "CHALLENGE_NOT_FOUND",
                    404);
            }

            // Map to response DTO
            return _mapper.Map<GetChallengesResp>(challenge);
        }


        public async Task<JoinChallengeResp> JoinChallenge(Guid challengeId, Guid runnerId)
        {
            var exists = await _dbContext.ChallengeParticipant
                .AnyAsync(cp => cp.ChallengeId == challengeId && cp.RunnerId == runnerId);

            if (exists)
                throw new BusinessException("Runner already joined this challenge", "ALREADY_JOINED", 409);

            var participation = new ChallengeParticipant
            {
                ChallengeId = challengeId,
                RunnerId = runnerId,
                JoinedAt = DateTime.UtcNow
            };

            _dbContext.ChallengeParticipant.Add(participation);
           var  particpantresp =  await _dbContext.SaveChangesAsync();

            return _mapper.Map<JoinChallengeResp>(participation);
        }
    }

}
