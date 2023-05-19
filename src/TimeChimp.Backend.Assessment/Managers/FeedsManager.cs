using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TimeChimp.Backend.Assessment.Enums;
using TimeChimp.Backend.Assessment.Helpers;
using TimeChimp.Backend.Assessment.Models;
using TimeChimp.Backend.Assessment.Repositories;

namespace TimeChimp.Backend.Assessment.Managers
{
    public class FeedsManager : IFeedsManager
    {
        private readonly ICacheService _cacheService;
        private readonly IDataAccessLayer _dataAccessLayer;
        private readonly ILogger _logger;

        public FeedsManager(IDataAccessLayerFactory dataAccessLayerFactory, ICacheService cacheService, ILogger<FeedsManager> logger)
        {
            this._cacheService = cacheService;
            this._logger = logger;
            // GetInstance of which mapper is going to be used. 
            this._dataAccessLayer = dataAccessLayerFactory.GetInstance(DataAccessLayerEnum.EntityFramework);
        }

        public async Task<Feed> GetFeedById(int feedId)
        {
            return await _dataAccessLayer.LogExceptionIfFail(_logger, async () => 
            {
                Feed feedResult;
                if (_cacheService.TryGetValue<Feed>(CacheKeysEnum.Feeds, out IEnumerable<Feed> feeds) && feeds.Any(f => f.Id == feedId))
                {
                    feedResult = feeds.Where(f => f.Id == feedId).FirstOrDefault();
                }
                else
                {
                    feedResult = await this._dataAccessLayer.GetFeedById(feedId);
                }
                return feedResult;
            });
        }

        public async Task<IEnumerable<Feed>> GetFeeds(QueryParameters queryParameters)
        {
            return await _dataAccessLayer.LogExceptionIfFail(_logger, async () =>
            {
                var existsParametersInCache = this._cacheService.TryGetValue<QueryParameters>(CacheKeysEnum.QueryParameters, out QueryParameters cacheQueryParameters);

                if (!_cacheService.TryGetValue<Feed>(CacheKeysEnum.Feeds, out IEnumerable<Feed> feeds) ||
                   (feeds.Any() && existsParametersInCache && !cacheQueryParameters.SamePropertiesAs(queryParameters)))
                {
                    this._cacheService.Set<QueryParameters>(CacheKeysEnum.QueryParameters, queryParameters);
                    feeds = await this._dataAccessLayer.GetFeeds(queryParameters);
                
                    this._cacheService.Set<Feed>(CacheKeysEnum.Feeds, feeds);
                } 
                else if(feeds.Any() && !cacheQueryParameters.SameSortAs(queryParameters))
                {
                    feeds = this._cacheService.OrderCache<Feed>(CacheKeysEnum.Feeds, queryParameters);
                }
            
                return feeds;
            });
        }
        
        public async Task<Feed> InsertFeed(Feed feed)
        {
            return await _dataAccessLayer.LogExceptionAndRollbackTransactionIfFail(_logger, async () =>
            { 
                var newFeed = await this._dataAccessLayer.InsertFeed(feed);

                if (newFeed.Id > 0)
                {
                    this._cacheService.Update<Feed>(CacheKeysEnum.Feeds, newFeed);
                    this._cacheService.OrderCache<Feed>(CacheKeysEnum.Feeds);
                }
                return newFeed;
            });
        }
    }
}
