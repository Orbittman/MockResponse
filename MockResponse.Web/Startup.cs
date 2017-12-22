using System;

using AutoMapper;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

using MockResponse.Core.Caching;
using MockResponse.Core.Data;
using MockResponse.Core.Models;
using MockResponse.Core.Utilities;
using MockResponse.Web.ApiClient;
using MockResponse.Web.Bootstrap;
using MockResponse.Web.Configuration;
using MockResponse.Web.Infrastructure;
using MockResponse.Web.Models;

using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;

using ServiceStack.Redis;

namespace MockResponse.Web
{
    public class Startup
    {
        readonly IConfigurationRoot _config;

        public Startup(IHostingEnvironment env)
		{
			// Set up configuration sources.
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", true, true)
                .AddJsonFile("wwwroot/Dist/assets.json")
		        .AddUserSecrets<Startup>();

            _config = builder.Build();
		}

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => { options.RespectBrowserAcceptHeader = true; });
            services.AddAutoMapper();
            services.AddRouting();

            ConfigureDI.Configure(services, _config);

            services.AddMailKit(optionBuilder =>
                                {
                                    optionBuilder.UseMailKit(new MailKitOptions
                                    {
                                        // get options from sercets.json
                                        Server = _config["SmtpServer"],
                                        Port = Convert.ToInt32(_config["SmtpPort"]),
                                        SenderName = _config["SmtpSenderName"],
                                        SenderEmail = _config["SmtpSenderEmail"],
                                        Account = _config["SmtpAccount"],
                                        Password = _config["SmtpPassword"]
                                    });
                                });

            // Configuration
            services.Configure<AppConfig>(_config);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDeveloperExceptionPage();
            app.UseMvc(ConfigureRoutes.Configure);
            app.UseStaticFiles();
        }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ResponseModel, ResponseViewModel>();
            CreateMap<ResponseViewModel, ResponseModel>();
        }
    }
}
