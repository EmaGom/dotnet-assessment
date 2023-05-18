using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TimeChimp.Backend.Assessment.Helpers
{
    public class CacheService : ICacheService
    {
        private readonly CacheConfiguration _cacheConfiguration;
        private readonly IMemoryCache _memoryCache;
        public CacheService(IMemoryCache memoryCache, IOptions<CacheConfiguration> cacheConfiguration)
        {
            this._memoryCache = memoryCache;
            this._cacheConfiguration = cacheConfiguration.Value;
        }
        public void Remove(string cacheKey)
        {
            this._memoryCache.Remove(cacheKey);    
        }

        public IEnumerable<T> Set<T>(string cacheKey, IEnumerable<T> value)
        {
            var memoryCacheEntryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(this._cacheConfiguration.AbsoluteExpirationInMinutes),
                SlidingExpiration = TimeSpan.FromMinutes(this._cacheConfiguration.SlidingExpirationInMinutes)
            };
            return this._memoryCache.Set(cacheKey, value, memoryCacheEntryOptions);
        }

        public T Set<T>(string cacheKey, T value)
        {
            var memoryCacheEntryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(this._cacheConfiguration.AbsoluteExpirationInMinutes),
                SlidingExpiration = TimeSpan.FromMinutes(this._cacheConfiguration.SlidingExpirationInMinutes)
            };
            return this._memoryCache.Set(cacheKey, value, memoryCacheEntryOptions);
        }

        public bool TryGetValue<T>(string cacheKey, out IEnumerable<T> value)
        {
            return _memoryCache.TryGetValue(cacheKey, out value);
        }

        public bool TryGetValue<T>(string cacheKey, out T value)
        {
            return _memoryCache.TryGetValue(cacheKey, out value);
        }

        public void Update<T>(string cacheKey, T value)
        {
            if(_memoryCache.TryGetValue(cacheKey, out IEnumerable<T> currentValue))
            {
                this.Set(cacheKey, currentValue.Append(value));
            } else
            {
                this.Set(cacheKey, new List<T>() { value });
            }
        }
    }
}
