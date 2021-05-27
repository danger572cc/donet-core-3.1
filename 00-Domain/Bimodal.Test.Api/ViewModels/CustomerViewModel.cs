using Bimodal.Test.Database;
using Kledex.Queries;
using System;
using System.Collections.Generic;

namespace Bimodal.Test.ViewModels
{
    public class CustomersViewModel : Query<IList<Customer>>
    {
    }

    public class CustomerViewModel : Query<Customer> 
    {
        public Guid ProductId { get; set; }
    }
}
