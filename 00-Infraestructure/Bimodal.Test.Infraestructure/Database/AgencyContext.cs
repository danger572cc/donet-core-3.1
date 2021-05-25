﻿using Microsoft.EntityFrameworkCore;

namespace Bimodal.Test.Infraestructure.Database
{
    public class AgencyContext : DbContext
    {
        public AgencyContext(DbContextOptions<AgencyContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<CustomerBooking> CustomerBookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerEntityTypeConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookingEntityTypeConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerBookingEntityTypeConfiguration).Assembly);
        }
    }
}
