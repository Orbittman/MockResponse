using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MockResponse
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()       
                .SetBasePath(Directory.GetCurrentDirectory())     
                .AddUserSecrets()
                .AddJsonFile($"hosting.json", optional: true)
                .Build();

            var host = new WebHostBuilder()
                .UseKestrel(options =>
                {
                    options.NoDelay = true;
                    //options.UseHttps("www.idldev.net.pfx", "xxxxxxxx");
                    options.UseConnectionLogging();
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(config)
                .UseStartup<Startup>()
                .Build();

                host.Run();
        }
    }
}
