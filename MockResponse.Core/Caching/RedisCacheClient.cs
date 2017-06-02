using System;

using ServiceStack.Redis;

namespace MockResponse.Core.Caching
{
    public class RedisCacheClient : ICacheClient
    {
        private readonly IRedisClient _cacheClient;

        public RedisCacheClient(IRedisClient cacheClient)
        {
            _cacheClient = cacheClient;
        }

        public void Delete(string key)
        {
            _cacheClient.Remove(key);
        }

        public TValue Get<TValue>(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                var entry = _cacheClient.Get<TValue>(key);
                if (entry != null)
                {
                    return entry;
                }
            }

            return default(TValue);
        }

        public void Set<TValue>(string key, TValue value)
        {
            _cacheClient.Set(key, value);
        }

        public void Set<TValue>(string key, TValue value, TimeSpan duration)
        {
            _cacheClient.Set(key, value, duration);
        }
    }
}
