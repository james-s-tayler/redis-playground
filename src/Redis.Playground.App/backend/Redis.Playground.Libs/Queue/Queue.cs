using System.Threading.Tasks;
using StackExchange.Redis;

namespace Redis.Playground.Libs.Queue
{
    public class Queue : IQueue
    {
        private readonly IDatabaseAsync _database;

        public Queue(IDatabaseAsync database)
        {
            _database = database;
        }

        public async Task<long> Push(string key, string value, bool fireAndForget = true)
        {
            if (fireAndForget)
            {
                return await _database.ListLeftPushAsync(FullKey(key), value, When.Always, CommandFlags.FireAndForget);     
            }

            return await _database.ListLeftPushAsync(FullKey(key), value);
        }

        public async Task<string> Pop(string key)
        {
            return await _database.ListRightPopAsync(FullKey(key));
        }

        public async Task<long> Size(string key)
        {
            return await _database.ListLengthAsync(FullKey(key));
        }

        private string FullKey(string key)
        {
            return $"queue:{key}";
        }
    }
}
