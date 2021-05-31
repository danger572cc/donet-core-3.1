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
    [Produces("application/json")]
    [Route("api/accounts/")]
    [ApiController]
    public class AccountController : BimodalBaseController
    {
        private readonly IDispatcher _dispatcher;

        private readonly IMapper _mapper;

        [HttpPost("register")]
        [ProducesResponseType(typeof(Detail), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create([FromBody] UserFormModel model)
        {
            var command = _mapper.Map<CreateUser>(model);
            await _dispatcher.SendAsync(command);
            var entityResult = new List<UserDTO>() { _mapper.Map<UserDTO>(command) };
            return CreatedAtAction(nameof(GetById), new { command.Id }, new Detail(StatusCodes.Status201Created, entityResult));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Detail), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                User result = await FindById(id);
                var viewModel = new List<UserDTO>() { _mapper.Map<UserDTO>(result) };
                return Ok(viewModel);
            }
            catch (ApplicationException ex)
            {
                var detail = ex.ToProblemDetails("User not found", StatusCodes.Status404NotFound);
                return new NotFoundObjectResult(detail);
            }
        }

        #region private methods
        private async Task<User> FindById(Guid id)
        {
            var query = new UserQueryModel()
            {
                Id = id
            };
            User result = await _dispatcher.GetResultAsync(query);
            await _dispatcher.GetResultAsync(new GetAggregateModel
            {
                AggregateRootId = id
            });
            return result;
        }
        #endregion
    }
}
