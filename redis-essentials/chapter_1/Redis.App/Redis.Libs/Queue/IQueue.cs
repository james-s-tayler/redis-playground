using System.Threading.Tasks;
using StackExchange.Redis;

namespace Redis.Libs.Queue
{
    public interface IQueue
    {
        Task<long> Push(string key, string value);
        Task<string> Pop(string key);
        Task<long> Size(string key);
    }
}