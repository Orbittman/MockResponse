using AutoMapper;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using MockResponse.Api.Commands;
using MockResponse.Api.Commands.Parameters;
using MockResponse.Api.Filters;
using MockResponse.Api.Models;
using MockResponse.Api.Queries;
using MockResponse.Core.Data;
using MockResponse.Core.Data.Models;
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
            services.AddAutoMapper();

			services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddScoped<IRequestContext, RequestContext>();
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();

            services.AddTransient<IResponseQuery, ResponseQuery>();
            services.AddTransient<IResponseCommand, ResponseCommand>();

            services.AddScoped<ThrottlingFilter>();
            services.Add(new ServiceDescriptor(typeof(IThrottler), typeof(Throttler), ServiceLifetime.Singleton));

            services.AddSingleton<INoSqlClient>(client => new MongoDbClient("mongodb://localhost:27017"));

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
            CreateMap<ResponseModel, ResponsePostParameters>(); ;
            CreateMap<ResponsePostParameters, Response>(); ;
        }
    }
}
