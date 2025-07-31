using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Request.Groups;
using Application.Models.Response.Groups;

namespace Application.Interfaces
{
    public interface IGroup
    {
        Task<CreateGroupResponse> CreateGroup(CreateGroupRequest createGroupRequest);
    }
}
