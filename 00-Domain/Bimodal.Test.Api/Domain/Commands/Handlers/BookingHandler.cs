using Bimodal.Test.Commands;
using Bimodal.Test.Database;
using Bimodal.Test.Events;
using Kledex.Commands;
using Kledex.Domain;
using Kledex.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bimodal.Test.Handlers
{
    public class BookingHandler : ICommandHandlerAsync<CreateBooking>,
        ICommandHandlerAsync<DeleteBooking>,
        ICommandHandlerAsync<UpdateBooking>,
        ICommandHandlerAsync<CreateTravelReserve>
    {
        private readonly AgencyContext _dbContext;

        public BookingHandler(AgencyContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<CommandResponse> HandleAsync(CreateBooking command)
        {
            var booking = new Booking(command.Id, command.BookingNumber, command.NumberOfPlaces, command.Origin, command.Destination, command.BasePrice);

            _dbContext.Bookings.Add(booking);

            await _dbContext.SaveChangesAsync();

            return new CommandResponse
            {
                Events = new List<IDomainEvent>()
                {
                    new BookingCreated
                    {
                        AggregateRootId = booking.Id,
                        BookingNumber = command.BookingNumber,
                        NumberOfPlaces = command.NumberOfPlaces,
                        Origin = command.Origin,
                        Destination = command.Destination,
                        BasePrice = command.BasePrice
                    }
                }
            };
        }

        public async Task<CommandResponse> HandleAsync(DeleteBooking command)
        {
            var booking = await _dbContext.Bookings.FirstOrDefaultAsync(x => x.Id == command.AggregateRootId);

            if (booking == null)
            {
                throw new ValidationException("id-Booking not found.");
            }

            _dbContext.Remove(booking);
            await _dbContext.SaveChangesAsync();

            return new CommandResponse
            {
                Events = new List<IDomainEvent>()
                {
                    new BookingDeleted
                    {
                        AggregateRootId = booking.Id
                    }
                }
            };
        }

        public async Task<CommandResponse> HandleAsync(UpdateBooking command)
        {
            var booking = await _dbContext.Bookings.FirstOrDefaultAsync(x => x.Id == command.AggregateRootId);

            if (booking == null)
            {
                throw new ValidationException("id-Booking not found.");
            }

            booking.Update(command.NumberOfPlaces, command.Origin, command.Destination, booking.BasePrice);

            await _dbContext.SaveChangesAsync();

            return new CommandResponse
            {
                Events = new List<IDomainEvent>()
                {
                    new BookingUpdated
                    {
                        AggregateRootId = booking.Id,
                        NumberOfPlaces = command.NumberOfPlaces,
                        Origin = command.Origin,
                        Destination = command.Destination,
                        BasePrice = command.BasePrice
                    }
                }
            };
        }

        public async Task<CommandResponse> HandleAsync(CreateTravelReserve command)
        {
            var reserve = new CustomerBooking(command.Id, command.CustomerId, command.BookingId);

            _dbContext.CustomerBookings.Add(reserve);

            await _dbContext.SaveChangesAsync();

            return new CommandResponse
            {
                Events = new List<IDomainEvent>()
                {
                    new TravelReserveCreated
                    {
                        AggregateRootId = reserve.Id,
                        CustomerId = reserve.CustomerId,
                        BookingId = reserve.BookingId,
                        CreatedAt = reserve.CreatedAt
                    }
                }
            };
        }
    }
}
