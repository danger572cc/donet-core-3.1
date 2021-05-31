using Bimodal.Test.Database;
using Kledex.Domain;
using System;

namespace Bimodal.Test.Commands
{
    public class CreateTravelReserve : DomainCommand<CustomerBooking>
    {
        public CreateTravelReserve() 
        {
            ValidateCommand = true;
        }

        public Guid CustomerId { get; set; }

        public Guid BookingId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
