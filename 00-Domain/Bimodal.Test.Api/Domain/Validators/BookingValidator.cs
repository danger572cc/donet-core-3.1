using Bimodal.Test.Api.Core.Extensions;
using Bimodal.Test.Commands;
using Bimodal.Test.Database;
using FluentValidation;

namespace Bimodal.Test.Api.Domain.Validators
{
    public class CreateBookingValidator : AbstractValidator<CreateBooking>
    {
        private readonly AgencyContext _dbContext;

        public CreateBookingValidator(AgencyContext agencyContext)
        {
            _dbContext = agencyContext;

            RuleFor(c => c.BookingNumber)
                .Must((documentNumber) => { return Utils.IsExistsBookingNumber(_dbContext, documentNumber); })
                .WithMessage("bookingNumber-BookingNumber is already registered.");
        }
    }
}
