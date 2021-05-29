using AutoMapper;
using Bimodal.Test.Api.Extensions;
using Bimodal.Test.Commands;
using Bimodal.Test.Common;
using Bimodal.Test.Database;
using Bimodal.Test.Handlers;
using Kledex;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        [HttpPost("")]
        [ProducesResponseType(typeof(Detail), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create([FromBody] CustomerFormModel model)
        {
            var command = _mapper.Map<CreateCustomer>(model);
            await _dispatcher.SendAsync(command);
            var entityResult = new List<CustomerDTO>() { _mapper.Map<CustomerDTO>(command) };
            return CreatedAtAction(nameof(GetById), new { command.Id }, new Detail(StatusCodes.Status201Created, entityResult));
        }

        /// <summary>
        /// Delete a customer by id.
        /// </summary>
        /// <param name="id">Customer id.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            try
            {
                var command = new DeleteCustomer
                {
                    AggregateRootId = id
                };
                await _dispatcher.SendAsync(command);
                return Ok(StatusCodes.Status204NoContent);
            }
            catch (ApplicationException ex)
            {
                var detail = ex.ToProblemDetails("Customer not found", StatusCodes.Status404NotFound);
                return new NotFoundObjectResult(detail);
            }
        }

        /// <summary>
        /// List all customers.
        /// </summary>
        [HttpGet("")]
        [ProducesResponseType(typeof(Detail), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var query = new CustomersListModel();
            IList<Customer> result = await _dispatcher.GetResultAsync(query);
            var viewModel = _mapper.Map<List<CustomerDTO>>(result);
            return Ok(viewModel);
        }

        /// <summary>
        /// Find a customer by id.
        /// </summary>
        /// <param name = "id" > Customer id.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Detail), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                Customer result = await FindById(id);
                var viewModel = new List<CustomerDTO>() { _mapper.Map<CustomerDTO>(result) };
                return Ok(viewModel);
            }
            catch (ApplicationException ex)
            {
                var detail = ex.ToProblemDetails("Customer not found", StatusCodes.Status404NotFound);
                return new NotFoundObjectResult(detail);
            }
        }

        /// <summary>
        /// Update customer information.
        /// </summary>
        /// <param name="model">Form with basic data of customer.</param>
        [HttpPut("")]
        [ProducesResponseType(typeof(Detail), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] CustomerUpdateFormModel model)
        {
            var command = _mapper.Map<UpdateCustomer>(model);
            await _dispatcher.SendAsync(command);
            return Ok(StatusCodes.Status204NoContent);
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
