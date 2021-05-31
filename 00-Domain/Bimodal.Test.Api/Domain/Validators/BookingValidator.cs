using Bimodal.Test.Api.Extensions;
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

    public class CreateTravelReserveValidator : AbstractValidator<CustomerBooking> 
    {
        private readonly AgencyContext _dbContext;

        public CreateTravelReserveValidator(AgencyContext agencyContext)
        {
            _dbContext = agencyContext;

            RuleFor(c => c.CustomerId)
                .Must((customerId) => { return Utils.IsExistsCustomer(_dbContext, customerId); })
                .WithMessage("customerId-Customer not found.");


            RuleFor(c => c.BookingId)
                .Must((bookingId) => { return Utils.IsExistsBookingInfo(_dbContext, bookingId); })
                .WithMessage("bookingId-Booking info not found.");
        }
    }
}
