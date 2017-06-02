using System.IO;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace MockResponse.Site
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("hosting.json", optional: false)
                .AddCommandLine(args)
                .Build();

            var host = new WebHostBuilder()
                .UseKestrel(
                    options =>
                    {
                        options.NoDelay = true;
                        //options.UseHttps("www.idldev.net.pfx", "xxxxxxxx");
                        options.UseConnectionLogging();
                    })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(config)
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
