using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using Microsoft.Extensions.Logging;
using TimeChimp.Backend.Assessment.Models;

namespace TimeChimp.Backend.Assessment.Repositories
{
    public class DataAccessLayerDapper : IDataAccessLayer
    {
        private readonly IContext _context;

        public DataAccessLayerDapper(IContext context)
        {
            this._context = context;
        }
        public async Task<Feed> GetFeedById(int feedId)
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = Queries.GetFeedById;
                var param = new
                {
                    feedId
                };
                return await connection.QuerySingleOrDefaultAsync<Feed>(sql, param);
            }
        }

        public async Task<IEnumerable<Feed>> GetFeeds(QueryParameters queryParameters)
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = Queries.GetFeeds;
                var param = new
                {
                    queryParameters.Title,
                    queryParameters.PublishDate,
                    queryParameters.SortBy,
                    queryParameters.SortDirection,
                    queryParameters.PageSize,
                    queryParameters.PageIndex
                };
                return await connection.QueryAsync<Feed, Category, Feed>(
                    sql,
                    (feed, category) =>
                    {
                        feed.Category = category;
                        return feed;
                    }, 
                    param,
                    splitOn: $"{nameof(Category.Name)}");
            }
        }

        public async Task<Category> InsertCategory(Category category)
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = Queries.InsertCategory;
                var param = new
                {
                    category.Name,
                    category.LastBuildDate
                };
                return await connection.QuerySingleAsync<Category>(sql, param);
            }
        }

        public async Task<Feed> InsertFeed(Feed feed)
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = Queries.InsertFeed;
                var param = new
                {
                   feed.PublishDate,
                   feed.Title,
                   feed.Url
                };
                return await connection.QuerySingleAsync<Feed>(sql, param);
            }
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
