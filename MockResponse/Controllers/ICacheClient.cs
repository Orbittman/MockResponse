namespace MockResponse.Site.Controllers
{
    public interface ICacheClient
    {
        TValue Get<TValue>(string key);

        void Set<TValue>(string key, TValue value);
    }
}