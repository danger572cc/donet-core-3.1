using Bimodal.Test.Api.Core.Extensions;
using Bimodal.Test.Commands;
using Bimodal.Test.Database;
using FluentValidation;

namespace Bimodal.Test.Api.Domain.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUser>
    {
        private readonly AgencyContext _dbContext;

        public CreateUserValidator(AgencyContext agencyContext)
        {
            _dbContext = agencyContext;

            RuleFor(c => c.UserName)
                .Must((username) => { return Utils.IsExistsCustomer(_dbContext, username); })
                .WithMessage("userName-Username is already registered.");

            RuleFor(c => c.Email)
                .Must((email) => { return Utils.IsExistsCustomer(_dbContext, email); })
                .WithMessage("email-Email is already registered.");
        }
    }
}
