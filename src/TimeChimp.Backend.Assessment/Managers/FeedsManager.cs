using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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

        public FeedsManager(IDataAccessLayer dataAccessLayer, ICacheService cacheService, ILogger<FeedsManager> logger)
        {
            this._cacheService = cacheService;
            this._logger = logger;
            this._dataAccessLayer = dataAccessLayer;
        }

        public async Task<Feed> GetFeedById(int feedId)
        {
            return await _dataAccessLayer.LogExceptionIfFail(_logger, async () => 
            {
                Feed feedResult;
                if (_cacheService.TryGetValue<Feed>(CacheKeys.Feeds, out IEnumerable<Feed> feeds) && feeds.Any(f => f.Id == feedId))
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
                var existsParametersInCache = this._cacheService.TryGetValue<QueryParameters>(CacheKeys.QueryParameters, out QueryParameters cacheQueryParameters);

                if (!_cacheService.TryGetValue<Feed>(CacheKeys.Feeds, out IEnumerable<Feed> feeds) ||
                   (feeds.Any() && existsParametersInCache && !cacheQueryParameters.SamePropertiesAs(queryParameters)))
                {
                    this._cacheService.Set<QueryParameters>(CacheKeys.QueryParameters, queryParameters);
                    feeds = await this._dataAccessLayer.GetFeeds(queryParameters);
                
                    this._cacheService.Set<Feed>(CacheKeys.Feeds, feeds);
                } 
            
                return feeds;
            });
        }

        public async Task<Feed> InsertFeed(Feed feed)
        {
            return await _dataAccessLayer.LogExceptionAndRollbackTransactionIfFail(_logger, async () =>
            { 
                Feed newFeed = null;
                var result = await this._dataAccessLayer.InsertFeed(feed);

                if (result > 0)
                {
                    this._cacheService.Remove(CacheKeys.Feeds);
                    newFeed = await this._dataAccessLayer.GetFeedById(result);
                }
                return newFeed;
            });
        }
    }
}
