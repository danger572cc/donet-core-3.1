using Bimodal.Test.Database;
using Kledex.Exceptions;
using Kledex.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bimodal.Test.Handlers
{
    public class CustomerQueryHandler : 
        IQueryHandlerAsync<CustomersListModel, IList<Customer>>, 
        IQueryHandlerAsync<CustomerQueryModel, Customer>
    {
        private readonly AgencyContext _dbContext;

        public CustomerQueryHandler(AgencyContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<Customer>> HandleAsync(CustomersListModel query)
        {
            return await _dbContext.Customers.ToListAsync();
        }

        public async Task<Customer> HandleAsync(CustomerQueryModel query)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == query.Id);

            if (customer == null)
            {
                throw new ValidationException("id-Customer not found.");
            }

            return customer;
        }
    }
}
