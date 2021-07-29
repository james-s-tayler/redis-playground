using Redis.Playground.Api.Domain.Auth.Models;

namespace Redis.Playground.Api.Domain.Auth.Requests
{
    public class LoginUserRequest
    {
        public LoginUser User { get; set; }
    }
}