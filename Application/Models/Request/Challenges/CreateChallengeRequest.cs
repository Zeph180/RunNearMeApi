using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Models.Request.Challenges
{
    public class CreateChallengeRequest
    {
        //public Guid GroupId { get; set; }
        [Required]
        public Guid RunnerId { get; set; }
        [MaxLength(100)]
        [Required]
        public required string Name { get; set; }
        [MaxLength(100)]
        [Required]
        public required string Description { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public bool IsDeleted { get; set; }
        //public DateTime? DeletedAt { get; set; }
        //public ICollection<Profile>? Profiles { get; set; }

    }
}
