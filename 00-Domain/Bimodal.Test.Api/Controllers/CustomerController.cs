using Bimodal.Test.Commands;
using Bimodal.Test.Database;
using Bimodal.Test.ViewModels;
using Kledex;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bimodal.Test.Api.Controllers
{
    [Route("api/customers/")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public CustomerController(IDispatcher dispatcher) 
        {
            _dispatcher = dispatcher;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CustomerFormModel model)
        {
            var command = new CreateCustomer 
            {
                Id = Guid.NewGuid(),
                DocumentNumber = model.Dni,
                Address = model.Address,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber
            };
            await _dispatcher.SendAsync(command);
            return CreatedAtAction(nameof(Create), new { CustomerId = command.Id }, command);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var query = new CustomersViewModel();
            IList<Customer> result = await _dispatcher.GetResultAsync(query);
            var viewModel = result.Select(s => new { s.Id, s.Dni, s.FullName, s.Address, s.PhoneNumber });
            return Ok(viewModel);
        }
    }
}
