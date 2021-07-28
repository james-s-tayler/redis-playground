namespace Redis.Playground.Api.Models.Auth
{
    public class User
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
        public string Token { get; set; }
    }
}