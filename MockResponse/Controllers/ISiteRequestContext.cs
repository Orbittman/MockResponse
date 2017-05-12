namespace MockResponse.Site.Controllers
{
    public interface ISiteRequestContext
    {
        void SaveUserSession(UserSession userSession);

        bool Authenticated { get; }
    }
}