using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Voting.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VoteController : ControllerBase
    {

        private readonly ILogger<VoteController> _logger;
        private readonly IConnectionMultiplexer _redis;

        public VoteController(ILogger<VoteController> logger, 
            IConnectionMultiplexer redis)
        {
            _logger = logger;
            _redis = redis;
        }
        
        [HttpGet("Info/{id}")]
        public async Task<string> Info(int id)
        {
            var info = await _redis.GetDatabase(0).StringGetAsync(new []{ new RedisKey($"article:{id}:headline"), new RedisKey($"article:{id}:votes")});
            return $"{info[0]} has {info[1]} vote(s)";
        }

        [HttpGet("Count/{id}")]
        public async Task<string> Get(int id)
        {
            return await _redis.GetDatabase(0).StringGetAsync($"article:{id}:votes");
        }
        
        [HttpGet("Up/{id}")]
        public async Task<long> Upvote(int id)
        {
            return await _redis.GetDatabase(0).StringIncrementAsync($"article:{id}:votes");
        }
        
        [HttpGet("Down/{id}")]
        public async Task<long> Downvote(int id)
        {
            return await _redis.GetDatabase(0).StringDecrementAsync($"article:{id}:votes");
        }
    }
}