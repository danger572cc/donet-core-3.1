using Bimodal.Test.Commands;
using FluentValidation;

namespace Bimodal.Test.Validators
{
    public class CreateCustomerValidator : AbstractValidator<CreateCustomer>
    {
        public CreateCustomerValidator() 
        {
            RuleFor(c => c.DocumentNumber)
                .GreaterThan(0)
                .WithMessage("Document number is required.");
                //.Must(IsExistsCustomer)
                //.WithMessage("Document number is already registered.");

            RuleFor(c => c.FullName)
                .NotEmpty()
                .WithMessage("Fullname is required.");

            RuleFor(c => c.Address)
                .NotEmpty()
                .WithMessage("Address is required.");
        }

        #region private methods
        //private bool IsExistsCustomer(int documentNumber) 
        //{
        //    bool result = _dbContext.Customers.Any(f => f.Dni == documentNumber);
        //    return result;
        //} 
        #endregion
    }
}
