using Kledex.Domain;

namespace Bimodal.Test.Events
{
    public class BookingCreated : DomainEvent
    {
        public string BookingNumber { get; set; }

        public int NumberOfPlaces { get; set; }

        public string Origin { get; set; }

        public string Destination { get; set; }

        public decimal BasePrice { get; set; }
    }
}
