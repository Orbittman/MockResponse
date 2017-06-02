using System;

namespace MockResponse.Core.Caching
{
    public interface ICacheClient
    {
        TValue Get<TValue>(string key);

        void Set<TValue>(string key, TValue value);

        void Set<TValue>(string key, TValue value, TimeSpan duration);

        void Delete(string key);
    }
}