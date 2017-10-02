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
		        .AddUserSecrets<Startup>();

            _config = builder.Build();
		}

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => { options.RespectBrowserAcceptHeader = true; });
            services.AddAutoMapper();
            services.AddRouting();

            services.AddSingleton<INoSqlClient>(client => new MongoDbClient($"mongodb://{_config["MongoUsername"]}:{_config["MongoPassword"]}@cluster0-shard-00-00-zlhjf.mongodb.net:27017,cluster0-shard-00-01-zlhjf.mongodb.net:27017,cluster0-shard-00-02-zlhjf.mongodb.net:27017/<DATABASE>?ssl=true&replicaSet=Cluster0-shard-0&authSource=admin"));
			services.AddTransient(c => new RedisManagerPool("localhost:6379").GetClient());
			services.AddSingleton<ICacheClient, RedisCacheClient>();
			services.AddSingleton<IRestClient, Client>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IEmailClient, EmailClient>();
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<ISiteRequestContext, SiteRequestContext>();

            services.AddMailKit(optionBuilder =>
                                {
                                    optionBuilder.UseMailKit(new MailKitOptions
                                    {
                                        //get options from sercets.json
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
