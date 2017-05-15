using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

using MockResponse.Core.Caching;
using MockResponse.Core.Data;
using MockResponse.Core.Utilities;
using MockResponse.Site.Bootstrap;
using MockResponse.Site.Controllers;

using ServiceStack.Redis;

namespace MockResponse.Site
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => { options.RespectBrowserAcceptHeader = true; });
            services.AddRouting();

            services.AddSingleton<INoSqlClient>(client => new MongoDbClient("mongodb://localhost:27017"));
            services.AddTransient(c => new RedisManagerPool("localhost:6379").GetClient());
            services.AddSingleton<ICacheClient, RedisCacheClient>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IEmailClient, EmailClient>();
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<ISiteRequestContext, SiteRequestContext>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDeveloperExceptionPage();
            app.UseMvc(ConfigureRoutes.Configure);
        }
    }
}
