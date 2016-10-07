using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

using AutoMapper;

namespace MockResponse
{
    public class Startup
    {
        private Response _defaultResponse = new Response{ ContentType="application/text", StatusCode = 400, Content = "This is the default response" };

        public void ConfigureServices(IServiceCollection services){
             services.AddMvc(options => 
             {
                 options.RespectBrowserAcceptHeader = true;
             });

            Mapper.Initialize(c => {});
            services.AddSingleton(Mapper.Configuration);
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<IConfigurationProvider>(), sp.GetService));
            services.AddAutoMapper();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
            app.Run(ReturnConfiguredResponse);           
        }

        private Task ReturnConfiguredResponse(HttpContext context) {
            return context.Response.WriteAsync("Found key");
        }

        private Task AddResponse(HttpContext context) {
            using(var db = new SqlLiteContext())
            {
                try{
                db.Responses.Add(new Response{ContentType = "application/json", StatusCode=200});
                db.SaveChanges();
                }catch(Exception ex){
                    var a = ex.Message;
                }
            }

            return context.Response.WriteAsync("Added new response");
        }
    }
}