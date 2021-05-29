using Bimodal.Test.Commands;
using Bimodal.Test.Database;
using Bimodal.Test.Events;
using Kledex.Commands;
using Kledex.Domain;
using Kledex.Exceptions;
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
                throw new ValidationException("id-Customer not found.");
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
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == command.AggregateRootId);

            if (customer == null)
            {
                throw new ValidationException("id-Customer not found.");
            }

            customer.Update(command.FullName, command.Address, command.PhoneNumber);

            await _dbContext.SaveChangesAsync();

            return new CommandResponse
            {
                Events = new List<IDomainEvent>()
                {
                    new CustomerUpdated
                    {
                        AggregateRootId = customer.Id,
                        FullName = customer.FullName,
                        Address = customer.Address,
                        PhoneNumber = customer.PhoneNumber
                    }
                }
            };
        }
    }
}
