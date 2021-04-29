using System;
using System.Linq;
using Redis.Libs.Queue;
using StackExchange.Redis;

namespace Redis.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || !args.Any() || string.IsNullOrEmpty(args[0]))
                throw new Exception("Please specify a queue name");

            var queueName = args[0];
            
            var connectionMultiplexer = ConnectionMultiplexer.Connect("localhost");
            var queue = new Queue(connectionMultiplexer.GetDatabase(0));
            
            Console.WriteLine($"Draining queue {queueName}!");
            
            while (queue.Size(queueName).GetAwaiter().GetResult() > 0)
            {
                var queueMessage = queue.Pop(queueName).GetAwaiter().GetResult();
                Console.WriteLine(queueMessage);
            }
            
            Console.WriteLine($"Finished draining queue {queueName}");
        }
    }
}