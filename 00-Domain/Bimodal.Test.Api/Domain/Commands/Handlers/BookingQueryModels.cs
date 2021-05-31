using Bimodal.Test.Database;
using Kledex.Queries;
using System;
using System.Collections.Generic;

namespace Bimodal.Test.Handlers
{
    public class BookingsListModel : Query<IList<Booking>>
    {
    }

    public class BookingQueryModel : Query<Booking> 
    {
        public Guid Id { get; set; }
    }
}
