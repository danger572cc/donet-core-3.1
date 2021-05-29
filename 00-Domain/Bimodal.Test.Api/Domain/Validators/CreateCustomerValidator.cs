using Bimodal.Test.Commands;
using Bimodal.Test.Database;
using FluentValidation;
using System.Linq;

namespace Bimodal.Test.Validators
{
    public class CreateCustomerValidator : AbstractValidator<CreateCustomer>
    {
        private readonly AgencyContext _dbContext;

        public CreateCustomerValidator(AgencyContext agencyContext) 
        {
            _dbContext = agencyContext;

            RuleFor(c => c.DocumentNumber)
                .NotEmpty()
                .WithMessage("dni-Document number is required.")
                .Must(IsExistsCustomer)
                .WithMessage("dni-Document number is already registered.");

            RuleFor(c => c.FullName)
                .NotEmpty()
                .WithMessage("fullName-Fullname is required.");

            RuleFor(c => c.Address)
                .NotEmpty()
                .WithMessage("address-Address is required.");
        }

        #region private methods
        private bool IsExistsCustomer(string documentNumber)
        {
            bool result = _dbContext.Customers.Any(f => f.Dni == documentNumber);
            return !result;
        }
        #endregion
    }
}
