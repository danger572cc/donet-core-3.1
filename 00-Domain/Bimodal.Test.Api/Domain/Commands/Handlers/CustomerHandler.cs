using Bimodal.Test.Commands;
using Bimodal.Test.Database;
using Bimodal.Test.Events;
using Kledex.Commands;
using Kledex.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bimodal.Test.Handlers
{
    public class CustomerHandler : ICommandHandlerAsync<CreateCustomer>,
        ICommandHandlerAsync<DeleteCustomer>,
        ICommandHandlerAsync<UpdateCustomer>
    {
        private readonly AgencyContext _dbContext;

        public CustomerHandler(AgencyContext dbContext) 
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

        public async Task<CommandResponse> HandleAsync(DeleteCustomer command)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == command.AggregateRootId);

            if (customer == null)
            {
                throw new ApplicationException($"Customer not found. Id: {command.AggregateRootId}");
            }

            return new CommandResponse
            {
                Events = new List<IDomainEvent>()
                {
                    new CustomerDeleted
                    {
                        AggregateRootId = customer.Id
                    }
                }
            };
        }

        public async Task<CommandResponse> HandleAsync(UpdateCustomer command)
        {
            throw new NotImplementedException();
        }
    }
}
