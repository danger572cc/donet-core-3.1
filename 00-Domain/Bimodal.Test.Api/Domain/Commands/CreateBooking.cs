﻿using Bimodal.Test.Database;
using Kledex.Domain;

namespace Bimodal.Test.Commands
{
    public class CreateBooking : DomainCommand<Booking>
    {
        public CreateBooking() 
        {
            ValidateCommand = true;
        }

        public string BookingNumber { get; set; }

        public int NumberOfPlaces { get; set; }

        public string Origin { get; set; }

        public string Destination { get; set; }

        public decimal BasePrice { get; set; }
    }
}
