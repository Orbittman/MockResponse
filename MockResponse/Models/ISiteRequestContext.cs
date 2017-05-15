using MockResponse.Site.Controllers;

namespace MockResponse.Site.Models
{
    public interface ISiteRequestContext
    {
        void SaveUserSession(UserSession userSession);

        bool Authenticated { get; }

        UserSession Session { get; }
    }
}