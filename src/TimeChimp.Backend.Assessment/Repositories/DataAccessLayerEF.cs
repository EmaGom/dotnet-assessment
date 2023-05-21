using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Transactions;
using TimeChimp.Backend.Assessment.Models;

namespace TimeChimp.Backend.Assessment.Repositories
{
    public class DataAccessLayerEF : IDataAccessLayer
    {
        private readonly DbContext _dbContext;
        public DataAccessLayerEF(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Feed> GetFeedById(int feedId)
        {
             return await _dbContext.Set<Feed>().FindAsync(feedId);
        }

        public async Task<IEnumerable<Feed>> GetFeeds(QueryParameters queryParameters = null)
        {
            var result = await _dbContext.Set<Feed>().AsNoTracking().Where(feed => (string.IsNullOrEmpty(queryParameters.Title) || EF.Functions.Like(feed.Title, $"%{queryParameters.Title}%")) &&
                                        (DateTime.MinValue == queryParameters.PublishDate || EF.Functions.DateDiffDay(feed.PublishDate, queryParameters.PublishDate) != 0)).ToListAsync();

            return result.AsQueryable().OrderBy($"{queryParameters.SortBy} {queryParameters.SortDirection}")
                            .Skip(queryParameters.PageSize * queryParameters.PageIndex)
                            .Take(queryParameters.PageSize);
        }

        public async Task<Feed> InsertFeed(Feed feed)
        {
            await _dbContext.Set<Feed>().AddAsync(feed);
            _dbContext.SaveChanges();
            return feed;
        }

        public async Task<Category> InsertCategory(Category category)
        {
            await _dbContext.Set<Category>().AddAsync(category);
            _dbContext.SaveChanges();
            return category;
        }

        #region LogException
        public async Task<T> LogExceptionAndRollbackTransactionIfFail<T>(ILogger logger, Func<Task<T>> func)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    T response = await func();
                    scope.Complete();
                    return response;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{ex.Message} for {ex.Data}");
                throw;
            }
        }

        public async Task<T> LogExceptionIfFail<T>(ILogger logger, Func<Task<T>> func)
        {
            try
            {
                T response = await func();
                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{ex.Message} for {ex.Data}");
                throw;
            }
        }
        #endregion
    }
}
