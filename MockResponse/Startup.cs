using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

using MockResponse.Core.Caching;
using MockResponse.Core.Data;
using MockResponse.Core.Utilities;
using MockResponse.Site.Bootstrap;
using MockResponse.Site.Controllers;
using MockResponse.Site.Infrastructure;
using MockResponse.Site.Models;
using MockResponse.Site.Configuration;

using ServiceStack.Redis;
using MockResponse.Site.ApiClient;

namespace MockResponse.Site
{
    public class Startup
    {
        IConfigurationRoot _config;

        public Startup(IHostingEnvironment env)
		{
			// Set up configuration sources.
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _config = builder.Build();
		}

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => { options.RespectBrowserAcceptHeader = true; });
            services.AddRouting();

            services.AddSingleton<INoSqlClient>(client => new MongoDbClient("mongodb://localhost:27017"));
			services.AddTransient(c => new RedisManagerPool("localhost:6379").GetClient());
			services.AddSingleton<ICacheClient, RedisCacheClient>();
			services.AddSingleton<IRestClient, Client>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IEmailClient, EmailClient>();
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<ISiteRequestContext, SiteRequestContext>();

            // Configuration
            services.Configure<AppConfig>(_config);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDeveloperExceptionPage();
            app.UseMvc(ConfigureRoutes.Configure);
        }
    }
}
