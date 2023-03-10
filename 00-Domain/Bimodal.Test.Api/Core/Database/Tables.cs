using Kledex.Domain;
using System;
using System.Collections.Generic;

namespace Bimodal.Test.Database
{
    public class Customer : AggregateRoot
    {
        public Customer() { }

        public Customer(Guid id, string documentNumber, string fullName, string address, string phoneNumber) 
        {
            Id = id;
            Dni = documentNumber;
            FullName = fullName;
            Address = address;
            PhoneNumber = phoneNumber;
        }

        public string Dni { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public virtual ICollection<CustomerBooking> CustomerBookings { get; set; }

        public void Update(string fullName, string address, string phoneNumber) 
        {
            FullName = fullName;
            Address = address;
            PhoneNumber = phoneNumber;
        }
    }

    public class Booking : AggregateRoot
    {
        public Booking() { }

        public Booking(Guid id, string bookingNumber, int numberOfPlaces, string origin, string destination, decimal basePrice)
        {
            Id = id;
            BookingNumber = bookingNumber;
            NumberOfPlaces = numberOfPlaces;
            Origin = origin;
            Destination = destination;
            BasePrice = basePrice;
        }

        public string BookingNumber { get; set; }

        public int NumberOfPlaces { get; set; }

        public string Origin { get; set; }

        public string Destination { get; set; }

        public decimal BasePrice { get; set; }

        public virtual ICollection<CustomerBooking> CustomerBookings { get; set; }

        public void Update(int numberOfPlaces, string origin, string destination, decimal basePrice)
        {
            NumberOfPlaces = numberOfPlaces;
            Origin = origin;
            Destination = destination;
            BasePrice = basePrice;
        }
    }

    public class CustomerBooking : AggregateRoot
    {
        public CustomerBooking() { }

        public CustomerBooking(Guid id, Guid customerId, Guid bookingId) 
        {
            Id = id;
            CustomerId = customerId;
            BookingId = bookingId;
            CreatedAt = DateTime.Now;
        }

        public Guid CustomerId { get; set; }

        public Guid BookingId { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual Customer Customers { get; set; }

        public virtual Booking Bookings { get; set; }
     }

    public class User : AggregateRoot
    {
        public User() { }

        public User(Guid id, string userName, string email, string passwordSalt, string passwordHash) 
        {
            Id = id;
            UserName = userName;
            Email = email;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            IsActive = true;
        }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PasswordSalt { get; set; }

        public string PasswordHash { get; set; }

        public bool IsActive { get; set; }
    }
}
