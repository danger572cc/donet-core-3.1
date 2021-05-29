using Bimodal.Test.Database;
using Kledex.Queries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bimodal.Test.Handlers
{
    public class CustomersListModel : Query<IList<Customer>>
    {
    }

    public class CustomerQueryModel : Query<Customer> 
    {
        public Guid Id { get; set; }
    }
}
