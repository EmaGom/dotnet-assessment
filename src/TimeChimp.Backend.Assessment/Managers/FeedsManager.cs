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
        private readonly IReaderService _readerService;
        private readonly ICacheService _cacheService;
        private readonly IDataAccessLayer _dataAccessLayer;
        private readonly ILogger _logger;

        public FeedsManager(IReaderService readerService, IDataAccessLayerFactory dataAccessLayerFactory, ICacheService cacheService, ILogger<FeedsManager> logger)
        {
            this._readerService = readerService;
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
        
        public async Task<Category> InsertFeeds(string categoryName)
        {
            return await _dataAccessLayer.LogExceptionAndRollbackTransactionIfFail(_logger, async () =>
            {
                Category category = _readerService.Read(categoryName);
                category = await this._dataAccessLayer.InsertCategory(category);

                if (category.Feeds.Any())
                {
                    foreach(var feed in category.Feeds)
                    {
                        feed.CategoryId = category.Id;
                        await this._dataAccessLayer.InsertFeed(feed);
                    }
                    this._cacheService.Update<Feed>(CacheKeysEnum.Feeds, category.Feeds);
                    this._cacheService.OrderCache<Feed>(CacheKeysEnum.Feeds);
                }

                return category;
            });
        }
    }
}
