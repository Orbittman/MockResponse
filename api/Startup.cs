using AutoMapper;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MockResponse.Api.Filters;
using MockResponse.Core.Data;
using MockResponse.Core.Utilities;

namespace MockResponse.Api
{
    public class Startup
    {
        //private IHostingEnvironment _environment;

        //public Startup(IHostingEnvironment environment)
        //{
        //    _environment = environment;
        //}

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => { options.RespectBrowserAcceptHeader = true; });

            //Mapper.Initialize(c => { });
            //services.AddSingleton(Mapper.Configuration);
            //services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<IConfigurationProvider>(), sp.GetService));
            //services.AddAutoMapper();

            services.AddScoped<ThrottlingFilter>();
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.Add(new ServiceDescriptor(typeof(IThrottler), typeof(Throttler), ServiceLifetime.Singleton));

			services.AddScoped<IRequestContext, RequestContext>();
			services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<INoSqlClient>(client => new MongoDbClient("mongodb://localhost:27017"));

			services.AddScoped<AuthorisationFilterAttribute>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}
