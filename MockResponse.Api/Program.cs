using System.IO;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace MockResponse.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("hosting.json", optional: false)
				.AddCommandLine(args)
			    .AddUserSecrets("MockResponse-b4141464-f1e7-4271-926f-31de0bc5be7e")
                .Build();

			var host = new WebHostBuilder()
				.UseKestrel(
					options =>
					{
                        //options.NoDelay = true;
                        //options.UseHttps("www.idldev.net.pfx", "xxxxxxxx");
                        //options.UseConnectionLogging();
                        //options.ListenLocalhost(1234);

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
