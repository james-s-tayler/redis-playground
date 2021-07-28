using System.Net;
using Microsoft.AspNetCore.Mvc;
using Redis.Playground.Api.Models.Auth;

namespace Redis.Playground.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpPost]
        public ActionResult CreateUser([FromBody] NewUserRequest newUserRequest)
        {
            var user = new User
            {
                Username = newUserRequest.User.Username,
                Email = newUserRequest.User.Email
            };
            return StatusCode((int)HttpStatusCode.Created, new UserResponse
            {
                User = user
            });
        }
    }
}