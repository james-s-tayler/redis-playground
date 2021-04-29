using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Redis.Libs.Queue;

namespace Redis.App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueueController : ControllerBase
    {
        
        private readonly ILogger<VoteController> _logger;
        private readonly IQueue _queue;

        public QueueController(ILogger<VoteController> logger, IQueue queue)
        {
            _logger = logger;
            _queue = queue;
        }
        
        [HttpGet("Size/{name}")]
        public async Task<string> Size(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            
            var size = await _queue.Size(name);

            return $"{size} message(s) currently in the queue.";
        }
        
        [HttpGet("Push/{name}/{count}")]
        public async Task<string> Push(string name, int count)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            
            if(count <= 0 || count >= int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(count));

            var size = default(long);
            for (var i = 0; i < count; i++)
            {
                size = await _queue.Push(name, $"message #{i}");
            }

            return $"{size} message(s) currently in the queue.";
        }
        
    }
}
