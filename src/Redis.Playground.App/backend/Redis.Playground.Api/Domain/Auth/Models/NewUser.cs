namespace Redis.Playground.Api.Domain.Auth.Models
{
    public class NewUser
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}