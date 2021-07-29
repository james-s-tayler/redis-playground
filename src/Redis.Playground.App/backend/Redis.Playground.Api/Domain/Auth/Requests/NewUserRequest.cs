using MediatR;
using Redis.Playground.Api.Domain.Auth.Models;

namespace Redis.Playground.Api.Domain.Auth.Requests
{
    public class NewUserRequest : IRequest<UserResponse>
    {
        public NewUser User { get; set; }
    }
}