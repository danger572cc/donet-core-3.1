using AutoMapper;
using Bimodal.Test.Api.Extensions;
using Bimodal.Test.Commands;
using Bimodal.Test.Common;
using Bimodal.Test.Database;
using Bimodal.Test.Handlers;
using Kledex;
using Kledex.UI.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bimodal.Test.Api.Controllers
{
    /// <summary>
    /// Manages booking travel data.
    /// </summary>
    [Produces("application/json")]
    [Route("api/bookings/")]
    [ApiController]
    public class BookingsController : BimodalBaseController
    {
        private readonly IDispatcher _dispatcher;

        private readonly IMapper _mapper;

        public BookingsController(IDispatcher dispatcher, IMapper mapper)
        {
            _dispatcher = dispatcher;
            _mapper = mapper;
        }

        /// <summary>
        /// Register a new travel booking.
        /// </summary>
        /// <param name="model">Form with basic data of customer.</param>
        [HttpPost("")]
        [ProducesResponseType(typeof(Detail), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create([FromBody] BookingFormModel model)
        {
            var command = _mapper.Map<CreateBooking>(model);
            await _dispatcher.SendAsync(command);
            var entityResult = new List<BookingDTO>() { _mapper.Map<BookingDTO>(command) };
            return CreatedAtAction(nameof(GetById), new { command.Id }, new Detail(StatusCodes.Status201Created, entityResult));
        }

        /// <summary>
        /// Delete a travel booking by id.
        /// </summary>
        /// <param name="id">Booking id.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            try
            {
                var command = new DeleteBooking
                {
                    AggregateRootId = id
                };
                await _dispatcher.SendAsync(command);
                return Ok(StatusCodes.Status204NoContent);
            }
            catch (ApplicationException ex)
            {
                var detail = ex.ToProblemDetails("Booking not found", StatusCodes.Status404NotFound);
                return new NotFoundObjectResult(detail);
            }
        }

        /// <summary>
        /// List all travel bookings.
        /// </summary>
        [HttpGet("")]
        [ProducesResponseType(typeof(Detail), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var query = new BookingsListModel();
            IList<Booking> result = await _dispatcher.GetResultAsync(query);
            var viewModel = _mapper.Map<List<BookingDTO>>(result);
            return Ok(viewModel);
        }

        /// <summary>
        /// Find a travel booking by id.
        /// </summary>
        /// <param name = "id" >Booking id.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Detail), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                Booking result = await FindById(id);
                var viewModel = new List<BookingDTO>() { _mapper.Map<BookingDTO>(result) };
                return Ok(viewModel);
            }
            catch (ApplicationException ex)
            {
                var detail = ex.ToProblemDetails("Booking not found", StatusCodes.Status404NotFound);
                return new NotFoundObjectResult(detail);
            }
        }

        /// <summary>
        /// Update travel booking information.
        /// </summary>
        /// <param name="model">Form with basic data of customer.</param>
        [HttpPut("")]
        [ProducesResponseType(typeof(Detail), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] BookingUpdateFormModel model)
        {
            var command = _mapper.Map<UpdateBooking>(model);
            await _dispatcher.SendAsync(command);
            return Ok(StatusCodes.Status204NoContent);
        }

        #region private methods
        private async Task<Booking> FindById(Guid id)
        {
            var query = new BookingQueryModel()
            {
                Id = id
            };
            Booking result = await _dispatcher.GetResultAsync(query);
            await _dispatcher.GetResultAsync(new GetAggregateModel
            {
                AggregateRootId = id
            });
            return result;
        }
        #endregion
    }
}
