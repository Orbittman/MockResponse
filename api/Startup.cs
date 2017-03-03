using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

using api.Filters;

using AutoMapper;

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

            services.AddSingleton<INoSqlClient>(client => new MongoDbClient("mongodb://localhost:27017"));
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }        
    }
}