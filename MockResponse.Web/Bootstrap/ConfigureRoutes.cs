using System;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MockResponse.Web.Controllers;
using MockResponse.Web.Extensions;

namespace MockResponse.Web.Bootstrap
{
    public static class ConfigureRoutes
    {
        internal static void Configure(IRouteBuilder routes)
        {
			routes.AddRoute<AuthController>("PostLogin", "login", c => c.LoginRequest(null));
			routes.AddRoute<AuthController>("GetLogin", "login/{token}", c => c.Login(null));
			routes.AddRoute<AuthController>("Logout", "logout", c => c.Logout());
            routes.AddRoute<HomeController>("Default", string.Empty, c => c.Index());
        }
    }
}