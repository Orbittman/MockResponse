using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace MockResponse
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", optional: true)
                .Build();

            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseConfiguration(config)
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }

    public class ApiCOntroller{
        [HttpGet("responses")]
        public object GetResources(){
            return new {Name="Something"};
        }

        [HttpPost("responses")]
        public object PostResponses(ResponseModel model)
        {
            return new {ContentType=model.ContentType};
        }
    }
}
