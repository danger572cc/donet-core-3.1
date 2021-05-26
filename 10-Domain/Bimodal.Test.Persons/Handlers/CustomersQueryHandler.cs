using Bimodal.Test.Customers.ViewModels;
using Bimodal.Test.Infraestructure.Database;
using Kledex.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bimodal.Test.Customers.Handlers
{
    public class CustomersQueryHandler : IQueryHandlerAsync<CustomersViewModel, IList<Customer>>
    {
        private readonly AgencyContext _dbContext;

        public CustomersQueryHandler(AgencyContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<Customer>> HandleAsync(CustomersViewModel query)
        {
            return await _dbContext.Customers.ToListAsync();
        }
    }

    public class CustomerQueryHandler : IQueryHandlerAsync<CustomerViewModel, Customer>
    {
        private readonly AgencyContext _dbContext;

        public CustomerQueryHandler(AgencyContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Customer> HandleAsync(CustomerViewModel query)
        {
            var product = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == query.ProductId);

            if (product == null)
            {
                throw new ApplicationException($"Product not found. Id: {query.ProductId}");
            }

            return product;
        }
    }
}
