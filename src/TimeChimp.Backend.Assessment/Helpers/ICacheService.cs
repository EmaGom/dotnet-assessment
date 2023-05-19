using System.Collections.Generic;
using TimeChimp.Backend.Assessment.Enums;
using TimeChimp.Backend.Assessment.Models;

namespace TimeChimp.Backend.Assessment.Helpers
{
    public interface ICacheService
    {
        bool TryGetValue<T>(CacheKeysEnum cacheKey, out IEnumerable<T> value);
        bool TryGetValue<T>(CacheKeysEnum cacheKey, out T value);
        IEnumerable<T> Set<T>(CacheKeysEnum cacheKey, IEnumerable<T> value);
        T Set<T>(CacheKeysEnum cacheKey, T value);
        void Remove(CacheKeysEnum cacheKey);
        void Update<T>(CacheKeysEnum cacheKey, T value);
        IEnumerable<T> OrderCache<T>(CacheKeysEnum cacheKey, QueryParameters queryParameters = null);
    }
}
