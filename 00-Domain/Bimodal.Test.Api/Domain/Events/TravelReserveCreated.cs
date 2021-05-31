using Kledex.Domain;
using System;

namespace Bimodal.Test.Events
{
    public class TravelReserveCreated : DomainEvent
    {
        public Guid CustomerId { get; set; }

        public Guid BookingId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
