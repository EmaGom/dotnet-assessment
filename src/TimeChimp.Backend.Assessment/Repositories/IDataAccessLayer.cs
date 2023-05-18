using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using TimeChimp.Backend.Assessment.Models;
using System;
using Microsoft.Extensions.Logging;

namespace TimeChimp.Backend.Assessment.Repositories
{
    public interface IDataAccessLayer
    {
        /// <summary>
        /// wraps the function passed in a try/catch that will log any thrown exception,
        /// and a transaction scope that will only complete if no exceptions are thrown
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="logger">The logger to use any thrown Exceptions during the function invocation</param>
        /// <param name="func">The function to run inside of the try/catch and transaction scope</param>
        /// <returns>result of func</returns>
        Task<T> LogExceptionAndRollbackTransactionIfFail<T>(ILogger logger, Func<Task<T>> func);

        /// <summary>
        /// Wraps the function passed in a try/catch that will log any thrown exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="logger">The logger to use any thrown Exceptions during the function invocation</param>
        /// <param name="func"></param>
        /// <returns></returns>
        Task<T> LogExceptionIfFail<T>(ILogger logger, Func<Task<T>> func);
        Task<IEnumerable<Feed>> GetFeeds(QueryParameters queryParameters = null);
        Task<int> InsertFeed(Feed feed);
        Task<Feed> GetFeedById(int feedId);
    }
}
