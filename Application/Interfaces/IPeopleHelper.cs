using Application.Models.Request.People;
using Domain.Entities;

namespace Application.Interfaces;

public interface IPeopleHelper
{
   Task<Friend> GetExistingFriendRequestAsync(Guid currentUser, Guid requestedId, Guid? friendShipId = null,
      bool track = false);

   Task<Profile> GetValidProfileAsync(Guid runnerId, string errorCode, string errorMessage);
   Task<Friend> UpdateFriendRequestHelper(UpdateFriendShip request);
}