using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TimeChimp.Backend.Assessment.Enums;
using TimeChimp.Backend.Assessment.Models;

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

        public void Remove(CacheKeysEnum cacheKey)
        {
            this._memoryCache.Remove(cacheKey);    
        }

        public IEnumerable<T> Set<T>(CacheKeysEnum cacheKey, IEnumerable<T> value)
        {
            var memoryCacheEntryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(this._cacheConfiguration.AbsoluteExpirationInMinutes),
                SlidingExpiration = TimeSpan.FromMinutes(this._cacheConfiguration.SlidingExpirationInMinutes)
            };
            return this._memoryCache.Set(cacheKey, value, memoryCacheEntryOptions);
        }

        public T Set<T>(CacheKeysEnum cacheKey, T value)
        {
            var memoryCacheEntryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(this._cacheConfiguration.AbsoluteExpirationInMinutes),
                SlidingExpiration = TimeSpan.FromMinutes(this._cacheConfiguration.SlidingExpirationInMinutes)
            };
            return this._memoryCache.Set(cacheKey, value, memoryCacheEntryOptions);
        }

        public bool TryGetValue<T>(CacheKeysEnum cacheKey, out IEnumerable<T> value)
        {
            return _memoryCache.TryGetValue(cacheKey, out value);
        }

        public bool TryGetValue<T>(CacheKeysEnum cacheKey, out T value)
        {
            return _memoryCache.TryGetValue(cacheKey, out value);
        }

        public void Update<T>(CacheKeysEnum cacheKey, IEnumerable<T> value)
        {
            if(_memoryCache.TryGetValue(cacheKey, out IEnumerable<T> currentValue))
            {
                this.Remove(cacheKey);
                this.Set(cacheKey, currentValue.Union(value));
            } else
            {
                this.Set(cacheKey, value);
            }
        }
        public IEnumerable<T> OrderCache<T>(CacheKeysEnum cacheKey, QueryParameters queryParameters = null)
        {
            if(this.TryGetValue(cacheKey, out IEnumerable<T> value))
            {
                if(queryParameters == null && !this.TryGetValue<QueryParameters>(CacheKeysEnum.QueryParameters, out queryParameters))
                {
                    queryParameters = new QueryParameters();
                }
                value = value.AsQueryable().OrderBy($"{queryParameters.SortBy} {queryParameters.SortDirection}");
                this.Remove(cacheKey);
                this.Remove(cacheKey);
                this.Set(cacheKey, value);
                this.Set(CacheKeysEnum.QueryParameters, queryParameters);
            }

            return value;
        }

    }
}
