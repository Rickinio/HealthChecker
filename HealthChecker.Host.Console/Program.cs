namespace HealthChecker.Host.Console
{
    using HealthChecker.Implementation;
    using Newtonsoft.Json;
    using System;
    class Program
    {
        static void Main(string[] args)
        {
            var executor = new HealthCheckExecutor();
            var results = executor.RunTests().Result;

            foreach (var result in results)
            {
                Console.WriteLine($"Result: {JsonConvert.SerializeObject(result)}");
            }

            Console.ReadKey();
        }
    }
}
