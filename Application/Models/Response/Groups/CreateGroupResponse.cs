using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Response.Groups
{
    public class CreateGroupResponse
    {
        public Guid GroupId { get; set; }
        public DateTime CreatedAt { get; set; }
        public  string Name { get; set; }
        public  string Description { get; set; }

    }
}
