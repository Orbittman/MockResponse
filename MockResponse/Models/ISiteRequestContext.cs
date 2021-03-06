namespace MockResponse.Web.Models
{
    public interface ISiteRequestContext
    {
        void SaveUserSession(UserSession userSession);

        bool Authenticated { get; }

        UserSession Session { get; }

        string ApiKey { get; }

        void ClearSession();
    }
}