using System.Collections.Generic;

namespace MockResponse.Site.Controllers
{
    public class CacheClient : ICacheClient
    {
        private readonly Dictionary<string, object> _cache = new Dictionary<string, object>();

        public TValue Get<TValue>(string key)
        {
            try
            {
                return (TValue)_cache[key];
            }
            catch
            {
                return default(TValue);
            }
        }

        public void Set<TValue>(string key, TValue value)
        {
            if (_cache.ContainsKey(key))
            {
                _cache[key] = value;
            }
            else
            {
                _cache.Add(key, value);
            }
        }
    }
}