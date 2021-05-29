using AutoMapper;
using Bimodal.Test.Commands;
using Bimodal.Test.Common;
using Bimodal.Test.Database;
using Bimodal.Test.Models;
using Kledex;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bimodal.Test.Api.Extensions;

namespace Bimodal.Test.Api.Controllers
{
    /// <summary>
    /// Manages customer data.
    /// </summary>
    [Produces("application/json")]
    [Route("api/customers/")]
    [ApiController]
    public class CustomersController : BimodalBaseController
    {
        private readonly IDispatcher _dispatcher;

        private readonly IMapper _mapper;

        public CustomersController(IDispatcher dispatcher, IMapper mapper)
        {
            _dispatcher = dispatcher;
            _mapper = mapper;
        }

        /// <summary>
        /// Register a new customer.
        /// </summary>
        /// <param name="model">Form with basic data of customer.</param>
        [HttpPost("create")]
        [ProducesResponseType(typeof(Detail<CustomerViewModel>), StatusCodes.Status201Created)]
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
            var entityResult = new List<CustomerViewModel>() { _mapper.Map<CustomerViewModel>(command) };
            return CreatedAtAction(nameof(GetById), new { command.Id }, new Detail<CustomerViewModel>(StatusCodes.Status201Created, entityResult));
        }

        /// <summary>
        /// List all customers.
        /// </summary>
        [HttpGet("")]
        [ProducesResponseType(typeof(Detail<CustomerViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var query = new CustomersListModel();
            IList<Customer> result = await _dispatcher.GetResultAsync(query);
            var viewModel = _mapper.Map<List<CustomerViewModel>>(result);
            return Ok(viewModel);
        }

        /// <summary>
        /// Find a customer by id.
        /// </summary>
        /// <param name="id">Customer id.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Detail<CustomerViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                Customer result = await FindById(id);
                var viewModel = new List<CustomerViewModel>() { _mapper.Map<CustomerViewModel>(result) };
                return Ok(viewModel);
            }
            catch (ApplicationException ex) 
            {
                var detail = ex.ToProblemDetails("Customer not found", StatusCodes.Status404NotFound);
                return new NotFoundObjectResult(detail);
            }
        }

        #region private methods
        private async Task<Customer> FindById(Guid id) 
        {
            var query = new CustomerQueryModel()
            {
                Id = id
            };
            Customer result = await _dispatcher.GetResultAsync(query);
            return result;
        }
        #endregion
    }
}
