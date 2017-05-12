using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using MockResponse.Core.Data;
using MockResponse.Site.Bootstrap;
using MockResponse.Site.Controllers;

namespace MockResponse.Site
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => { options.RespectBrowserAcceptHeader = true; });
            services.AddRouting();

            services.AddSingleton<INoSqlClient>(client => new MongoDbClient("mongodb://localhost:27017"));
            services.AddSingleton<IEmailClient>(client => new EmailClient());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDeveloperExceptionPage();
            app.UseMvc(ConfigureRoutes.Configure);
        }
    }
}
