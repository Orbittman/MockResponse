using Microsoft.AspNetCore.Routing;

using MockResponse.Site.Controllers;
using MockResponse.Site.Extensions;

namespace MockResponse.Site.Bootstrap
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