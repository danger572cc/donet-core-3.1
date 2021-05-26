using Kledex.Domain;
using System;
using System.Collections.Generic;

namespace Bimodal.Test.Infraestructure.Database
{
    public class Customer : AggregateRoot
    {
        public Customer() { }

        public int Dni { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public virtual ICollection<CustomerBooking> CustomerBookings { get; set; }
    }

    public class Booking : AggregateRoot
    {
        public string BookingNumber { get; set; }

        public int NumberOfPlaces { get; set; }

        public string Origin { get; set; }

        public string Destination { get; set; }

        public decimal BasePrice { get; set; }

        public virtual ICollection<CustomerBooking> CustomerBookings { get; set; }
    }

    public class CustomerBooking
    {
        public string CustomerId { get; set; }

        public string BookingId { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual Customer Customers { get; set; }

        public virtual Booking Bookings { get; set; }
     }
}
