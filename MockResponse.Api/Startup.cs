using AutoMapper;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using MockResponse.Api.Commands;
using MockResponse.Api.Commands.Parameters;
using MockResponse.Api.Filters;
using MockResponse.Api.Models;
using MockResponse.Api.Queries;
using MockResponse.Core.Data;
using MockResponse.Core.Data.Models;
using MockResponse.Core.Models;
using MockResponse.Core.Utilities;

namespace MockResponse.Api
{
    public class Startup
    {
        readonly IConfigurationRoot _config;

        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                //.SetBasePath(env.ContentRootPath)
                //.AddJsonFile("appsettings.json", true, true)
                .AddUserSecrets<Startup>();

            _config = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => { options.RespectBrowserAcceptHeader = true; });
            services.AddAutoMapper();

			services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddScoped<IRequestContext, RequestContext>();
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();

            services.AddTransient<IResponseQuery, ResponseQuery>();
            services.AddTransient<IResponsesQuery, ResponsesQuery>();
            services.AddTransient<IAccountQuery, AccountQuery>();

			services.AddTransient<IPostResponseCommand, PostResponseCommand>();
			services.AddTransient<IResponseDeleteCommand, DeleteResponseCommand>();

            services.AddScoped<ThrottlingFilter>();
            services.Add(new ServiceDescriptor(typeof(IThrottler), typeof(Throttler), ServiceLifetime.Singleton));

            services.AddSingleton<INoSqlClient>(client => new MongoDbClient($"mongodb://{_config["MongoUsername"]}:{_config["MongoPassword"]}@cluster0-shard-00-00-zlhjf.mongodb.net:27017,cluster0-shard-00-01-zlhjf.mongodb.net:27017,cluster0-shard-00-02-zlhjf.mongodb.net:27017/IDL_Monitor?ssl=true&replicaSet=Cluster0-shard-0&authSource=admin"));

			services.AddScoped<AuthorisationFilterAttribute>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }
            app.UseMvc();
        }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ResponseModel, ResponsePostParameters>();
            CreateMap<ResponsePostParameters, Response>();
            CreateMap<Response, ResponseModel>();
        }
    }
}
