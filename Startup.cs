using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace MockResponse
{
    public class Startup
    {
        private Response _defaultResponse = new Response{ ContentType="application/text", HttpStatusCode = 400, Content = "This is the default response" };

        public void ConfigureServices(IServiceCollection services){
             services.AddRouting();
        }

        public void Configure(IApplicationBuilder app)
        {
            var routes = new Dictionary<string, Func<HttpContext>>();

            var responses = new Dictionary<string, Response>{{"/jhlahlkj/dsfsdf/dsfsdfs?gfgfg=ttttt", new Response{ ContentType="application/json", HttpStatusCode = 200, Content = "{\"bob\" : \"This is a found response\"}"}}};
            app.Run(context =>
            {
                var requestKey = $"{context.Request.Path}{context.Request.QueryString}";
                var response = responses.ContainsKey(requestKey) ? responses[requestKey] : _defaultResponse;

                context.Response.StatusCode = response.HttpStatusCode;
                context.Response.ContentType = response.ContentType;

                return context.Response.WriteAsync($"{response.Content}");
            });
        }
    }
}