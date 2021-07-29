using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Redis.Playground.Api.Domain.Auth.Models;
using Redis.Playground.Api.Domain.Auth.Requests;

namespace Redis.Playground.Api.Domain.Auth.Handlers
{
    public class NewUserRequestHandler : IRequestHandler<NewUserRequest, UserResponse>
    {
        public Task<UserResponse> Handle(NewUserRequest request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Username = request.User.Username,
                Email = request.User.Email
            };
            return Task.FromResult(new UserResponse
            {
                User = user
            });
        }
    }
}