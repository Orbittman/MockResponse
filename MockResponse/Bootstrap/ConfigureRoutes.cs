using System;

using Microsoft.AspNetCore.Routing;

using MockResponse.Site.Controllers;
using MockResponse.Site.Extensions;

namespace MockResponse.Site.Bootstrap
{
    public static class ConfigureRoutes
    {
        internal static void Configure(IRouteBuilder routes)
        {
            routes.AddRoute<HomeController>("PostLogin", "login", c => c.LoginRequest(null));
            routes.AddRoute<HomeController>("GetLogin", "login/{token}", c => c.Login(null));
            routes.AddRoute<HomeController>("Default", string.Empty, c => c.Index());
        }
    }
}
