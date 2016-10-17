using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using System.IO;
using System.Threading.Tasks;

using AutoMapper;

namespace MockResponse
{
    public class Startup
    {
        private IHostingEnvironment _environment;

        private Response _defaultResponse = new Response { ContentType = "application/text", StatusCode = 400, Content = "This is the default response" };

        public Startup(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // services.Configure<KestrelServerOptions>(options =>
            // {
            //     options.ThreadCount = 4;
            //     options.UseHttps(new X509Certificate2(...));
            //     options.UseConnectionLogging();
            // });

            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true;
            });

            Mapper.Initialize(c => { });
            services.AddSingleton(Mapper.Configuration);
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));
            services.AddAutoMapper();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
            app.Run(ReturnConfiguredResponse);
        }

        private Task ReturnConfiguredResponse(HttpContext context)
        {
            using (var db = new SqlLiteContext())
            {
                Response response = new Response();
                if (context.Request.Path.Value == "/")
                {
                    context.Response.StatusCode = 200;
                    response.Content = "Mock Response - OK";
                }
                else
                {
                    response = db.Responses.SingleOrDefault(d => d.Path == context.Request.Path.Value);
                    if (response == null)
                    {
                        context.Response.StatusCode = 400;
                    }
                    else
                    {
                        context.Response.StatusCode = response.StatusCode;
                        context.Response.ContentType = response.ContentType;
                        context.Response.Headers["content-encoding"] = response.ContentEncoding;
                        context.Response.Headers["Vary"] = response.Vary;
                        context.Response.Headers["cache-control"] = response.CacheControl;
                    }
                }

                return context.Response.WriteAsync(response?.Content ?? "Not found");
            }
        }
    }
}