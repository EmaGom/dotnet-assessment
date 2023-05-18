using System.Collections.Generic;

namespace TimeChimp.Backend.Assessment.Helpers
{
    public interface ICacheService
    {
        bool TryGetValue<T>(string cacheKey, out IEnumerable<T> value);
        bool TryGetValue<T>(string cacheKey, out T value);
        IEnumerable<T> Set<T>(string cacheKey, IEnumerable<T> value);
        T Set<T>(string cacheKey, T value);
        void Remove(string cacheKey);
        void Update<T>(string cacheKey, T value);
    }
}
