using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using System.Threading.Tasks;

using api.Filters;

using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace MockResponse
{
    public class Startup
    {
        private IHostingEnvironment _environment;

        public Startup(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true;
            });

            Mapper.Initialize(c => { });
            services.AddSingleton(Mapper.Configuration);
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<IConfigurationProvider>(), sp.GetService));
            services.AddAutoMapper();

            services.AddScoped<ThrottlingFilter>();
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.Add(new ServiceDescriptor(typeof(IThrottler), typeof(Throttler), ServiceLifetime.Singleton));
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
                var path = context.Request.Path.Value.TrimStart('/');
                if (path == string.Empty)
                {
                    context.Response.StatusCode = 200;
                    response.Content = "Mock Response - OK";
                }
                else
                {
                    response = db
                        .Responses
                        .Include(r => r.Headers)
                        .SingleOrDefault(d => d.Path == path && d.Domain.Name == context.Request.Host.Host);
                        
                    if (response == null)
                    {
                        context.Response.StatusCode = 404;
                    }
                    else
                    {
                        context.Response.StatusCode = response.StatusCode;
                        if (response.Headers != null)
                        {
                            foreach (var header in response.Headers)
                            {
                                context.Response.Headers[header.Name] = header.Value;
                            }
                        }
                    }
                }

                return context.Response.WriteAsync(response?.Content ?? "Not found");
            }
        }
    }
}