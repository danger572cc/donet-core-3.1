using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bimodal.Test.Infraestructure.Database
{
    public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(p => p.Id)
                    .IsRequired();
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Dni)
                   .IsRequired();
            builder.HasIndex(i => i.Dni)
                   .IsUnique();
            builder.Property(p => p.FullName)
                    .IsRequired();
            builder.Property(p => p.Address)
                    .IsRequired();
            builder.Property(p => p.PhoneNumber)
                    .IsRequired();
            builder.Ignore(p => p.Events);
            builder.Ignore(p => p.Version);
            builder.HasMany(m => m.CustomerBookings)
                   .WithOne(m => m.Customers)
                   .HasForeignKey(f => f.CustomerId);
        }
    }

    public class BookingEntityTypeConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Property(p => p.Id)
                    .IsRequired();
            builder.HasKey(p => p.Id);
            builder.Property(p => p.BookingNumber)
                   .IsRequired();
            builder.Property(p => p.NumberOfPlaces)
                    .IsRequired();
            builder.Property(p => p.Origin)
                    .IsRequired();
            builder.Property(p => p.Destination)
                    .IsRequired();
            builder.Property(p => p.BasePrice)
                    .IsRequired();
            builder.Ignore(p => p.Events);
            builder.Ignore(p => p.Version);
        }
    }

    public class CustomerBookingEntityTypeConfiguration : IEntityTypeConfiguration<CustomerBooking>
    {
        public void Configure(EntityTypeBuilder<CustomerBooking> builder)
        {
            builder.HasOne(m => m.Customers)
                   .WithMany(m => m.CustomerBookings)
                   .HasForeignKey(f => f.CustomerId);
            builder.HasOne(m => m.Bookings)
                   .WithMany(m => m.CustomerBookings)
                   .HasForeignKey(f => f.BookingId);
        }
    }

}
