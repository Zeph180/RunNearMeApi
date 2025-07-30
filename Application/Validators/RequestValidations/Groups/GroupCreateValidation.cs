using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Request.Groups;
using Domain.Models.Request.Account;
using FluentValidation;

namespace Application.Validators.RequestValidations.Groups
{
    public class GroupCreateValidation : AbstractValidator<CreateGroupRequest>
    {
        public GroupCreateValidation() 
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2, 50).WithMessage("Name must be between 2 and 50 characters");

            RuleFor(x => x.RunnerId)
               .NotEmpty().WithMessage("RunnerId is required");

            RuleFor(x => x.Description)
               .NotEmpty().WithMessage("Group Description is required");
        }
    }
}
