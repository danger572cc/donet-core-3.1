using Bimodal.Test.Commands;
using Bimodal.Test.Database;
using Bimodal.Test.Events;
using Kledex.Commands;
using Kledex.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bimodal.Test.Handlers
{
    public class CreateCustomerHandler : ICommandHandlerAsync<CreateCustomer>
    {
        private readonly AgencyContext _dbContext;

        public CreateCustomerHandler(AgencyContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<CommandResponse> HandleAsync(CreateCustomer command)
        {
            var customer = new Customer(command.Id, command.DocumentNumber, command.FullName, command.Address, command.PhoneNumber);

            _dbContext.Customers.Add(customer);

            await _dbContext.SaveChangesAsync();

            return new CommandResponse
            {
                Events = new List<IDomainEvent>()
                {
                    new CustomerCreated
                    {
                        AggregateRootId = customer.Id,
                        Dni = customer.Dni,
                        FullName = customer.FullName,
                        Address = customer.Address,
                        PhoneNumber = customer.PhoneNumber
                    }
                }
            };
        }
    }
}
