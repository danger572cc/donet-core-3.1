using Bimodal.Test.Database;
using Kledex.Exceptions;
using Kledex.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bimodal.Test.Handlers
{
    public class BookingQueryHandler : 
        IQueryHandlerAsync<BookingsListModel, IList<Booking>>, 
        IQueryHandlerAsync<BookingQueryModel, Booking>
    {
        private readonly AgencyContext _dbContext;

        public BookingQueryHandler(AgencyContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<Booking>> HandleAsync(BookingsListModel query)
        {
            return await _dbContext.Bookings.ToListAsync();
        }

        public async Task<Booking> HandleAsync(BookingQueryModel query)
        {
            var booking = await _dbContext.Bookings.FirstOrDefaultAsync(x => x.Id == query.Id);

            if (booking == null)
            {
                throw new ValidationException("id-Booking not found.");
            }

            return booking;
        }
    }
}
