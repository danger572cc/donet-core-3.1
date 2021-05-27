using Kledex.Domain;
using System;
using System.Collections.Generic;

namespace Bimodal.Test.Database
{
    public class Customer : AggregateRoot
    {
        public Customer() { }

        public Customer(Guid id, int documentNumber, string fullName, string address, int phoneNumber) 
        {
            Id = id;
            Dni = documentNumber;
            FullName = fullName;
            Address = address;
            PhoneNumber = phoneNumber;
        }

        public int Dni { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public int PhoneNumber { get; set; }

        public virtual ICollection<CustomerBooking> CustomerBookings { get; set; }

        public void Update(int documentNumber, string fullName, string address, int phoneNumber) 
        {
            Dni = documentNumber;
            FullName = fullName;
            Address = address;
            PhoneNumber = phoneNumber;
        }
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
        public Guid CustomerId { get; set; }

        public Guid BookingId { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual Customer Customers { get; set; }

        public virtual Booking Bookings { get; set; }
     }
}
