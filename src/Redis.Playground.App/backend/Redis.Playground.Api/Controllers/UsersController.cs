using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Redis.Playground.Api.Domain.Auth.Models;
using Redis.Playground.Api.Domain.Auth.Requests;

namespace Redis.Playground.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] NewUserRequest newUserRequest)
        {
            var userResponse = await _mediator.Send(newUserRequest);
            return StatusCode((int)HttpStatusCode.Created, userResponse);
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginUserRequest loginUserRequest)
        {
            if (false)
                return Unauthorized();
            
            return Ok();
        }
    }
}