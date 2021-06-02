using AutoMapper;
using Bimodal.Test.Api.Extensions;
using Bimodal.Test.Commands;
using Bimodal.Test.Common;
using Bimodal.Test.Database;
using Bimodal.Test.Handlers;
using Bimodal.Test.Token;
using Kledex;
using Kledex.UI.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bimodal.Test.Api.Controllers
{
    /// <summary>
    /// Manages single credentials
    /// </summary>
    [Produces("application/json")]
    [Route("api/accounts/")]
    [ApiController]
    public class AccountController : BimodalBaseController
    {
        private readonly IDispatcher _dispatcher;

        private readonly IMapper _mapper;

        private readonly IJwtAuthManager _jwtAuthManager;

        public AccountController(IDispatcher dispatcher, IMapper mapper, IJwtAuthManager jwtAuthManager)
        {
            _dispatcher = dispatcher;
            _mapper = mapper;
            _jwtAuthManager = jwtAuthManager;
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="model">Single form to register a user.</param>
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

        /// <summary>
        /// Get user by Id.
        /// </summary>
        /// <param name="id">User Id.</param>
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

        /// <summary>
        /// Login.
        /// </summary>
        /// <param name="request">Login form.</param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var query = new UserLoginModel()
            {
                UserName = request.UserName,
                Password = request.Password
            };
            User result = await _dispatcher.GetResultAsync(query);
            if (result == null)
            {
                var detail = new Detail(StatusCodes.Status401Unauthorized, new { Message = "Invalid credentials" });
                return Unauthorized(detail);
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, request.UserName),
                new Claim(ClaimTypes.Role, UserRoles.BASIC_USER)
            };

            var jwtResult = _jwtAuthManager.GenerateTokens(request.UserName, claims, DateTime.Now);
            var response = new LoginResultDTO
            {
                UserName = request.UserName,
                Role = UserRoles.BASIC_USER,
                AccessToken = jwtResult.AccessToken,
                Expires = jwtResult.ExpiresIn
            };
            return Ok(response);
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
