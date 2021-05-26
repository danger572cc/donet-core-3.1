using Bimodal.Test.Customers.Commands;
using Bimodal.Test.Infraestructure.Database;
using Kledex.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bimodal.Test.Customers.Handlers
{
    public class CreateCustomerHandler : ICommandHandlerAsync<CreateCustomer>
    {
        private readonly AgencyContext _dbContext;

        public CreateCustomerHandler(AgencyContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public Task<CommandResponse> HandleAsync(CreateCustomer command)
        {
            throw new NotImplementedException();
        }
    }
}
