using System;

namespace Bimodal.Test.Common
{
    public sealed class BookingDTO
    {
        public Guid Id { get; set; }

        public string BookingNumber { get; set; }

        public int NumberOfPlaces { get; set; }

        public string Origin { get; set; }

        public string Destination { get; set; }

        public decimal BasePrice { get; set; }
    }
}
