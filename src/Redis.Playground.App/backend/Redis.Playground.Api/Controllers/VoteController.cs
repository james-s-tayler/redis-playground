using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Redis.Playground.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VoteController : ControllerBase
    {

        private readonly ILogger<VoteController> _logger;
        private readonly IDatabaseAsync _redis;

        public VoteController(ILogger<VoteController> logger, 
            IDatabaseAsync redis)
        {
            _logger = logger;
            _redis = redis;
        }
        
        [HttpGet("Info/{id}")]
        public async Task<string> Info(int id)
        {
            var info = await _redis.StringGetAsync(new []{ new RedisKey($"article:{id}:headline"), new RedisKey($"article:{id}:votes")});
            return $"{info[0]} has {info[1]} vote(s)";
        }

        [HttpGet("Count/{id}")]
        public async Task<string> Get(int id)
        {
            return await _redis.StringGetAsync($"article:{id}:votes");
        }
        
        [HttpGet("Up/{id}")]
        public async Task<long> Upvote(int id)
        {
            return await _redis.StringIncrementAsync($"article:{id}:votes");
        }
        
        [HttpGet("Down/{id}")]
        public async Task<long> Downvote(int id)
        {
            return await _redis.StringDecrementAsync($"article:{id}:votes");
        }
    }
}
