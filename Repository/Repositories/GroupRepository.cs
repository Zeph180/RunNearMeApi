using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Models.Request.Groups;
using Application.Models.Response.Groups;

namespace Repository.Repositories
{
    public  class GroupRepository : IGroup
    {
        public Task<CreateGroupResponse> CreateGroup(CreateGroupRequest createGroupRequest)
        {
            // Implementation of group creation logic goes here
            throw new NotImplementedException();
        }

    }
}
