using System.Threading.Tasks;
using StackExchange.Redis;

namespace Redis.Playground.Libs.Queue
{
    public interface IQueue
    {
        Task<long> Push(string key, string value, bool fireAndForget = true);
        Task<string> Pop(string key);
        Task<long> Size(string key);
    }
}
