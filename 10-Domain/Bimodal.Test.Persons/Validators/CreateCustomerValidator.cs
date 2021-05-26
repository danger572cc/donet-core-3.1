using Bimodal.Test.Customers.Commands;
using FluentValidation;

namespace Bimodal.Test.Customers.Validators
{
    public class CreateCustomerValidator : AbstractValidator<CreateCustomer>
    {
        public CreateCustomerValidator() 
        {
            RuleFor(c => c.DocumentNumber)
                .GreaterThan(0)
                .WithMessage("Document number is required.");

            RuleFor(c => c.FullName)
                .NotEmpty()
                .WithMessage("Fullname is required.");

            RuleFor(c => c.Address)
                .NotEmpty()
                .WithMessage("Address is required.");
        }
    }
}
