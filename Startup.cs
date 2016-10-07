using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace MockResponse
{
    public class Startup
    {
        private Response _defaultResponse = new Response{ ContentType="application/text", HttpStatusCode = 400, Content = "This is the default response" };

        public void ConfigureServices(IServiceCollection services){
             services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
            //  var routeHandler = new RouteHandler(context =>
            // {
            //     return null;
            // });

            // var routeBuilder = new RouteBuilder(app, routeHandler);
            // routeBuilder.MapPost("api/response", AddResponse);

            // var routes = routeBuilder.Build();
            // app.UseRouter(routes);
            app.Run(context => context.Response.WriteAsync(context.Request.Path));
            //var responses = new Dictionary<string, Response>{{"/jhlahlkj/dsfsdf/dsfsdfs?gfgfg=ttttt", new Response{ ContentType="application/json", HttpStatusCode = 200, Content = "{\"bob\" : \"This is a found response\"}"}}};
            // app.Run(context =>
            // {
            //     var requestKey = $"{context.Request.Path}{context.Request.QueryString}";
            //     var response = responses.ContainsKey(requestKey) ? responses[requestKey] : _defaultResponse;

            //     context.Response.StatusCode = response.HttpStatusCode;
            //     context.Response.ContentType = response.ContentType;

            //     return context.Response.WriteAsync($"{response.Content}");
            // });
        }

        private Task AddResponse(HttpContext context) {
            using(var db = new SqlLiteContext())
            {
                try{
                db.Responses.Add(new Response{ContentType = "application/json", HttpStatusCode=200});
                db.SaveChanges();
                }catch(Exception ex){
                    var a = string.Empty;
                }
            }

            return context.Response.WriteAsync("Added new response");
        }
    }
}