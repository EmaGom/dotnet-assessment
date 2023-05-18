using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TimeChimp.Backend.Assessment.Models;

namespace TimeChimp.Backend.Assessment.Managers
{
    public interface IFeedsManager
    {
        public Task<Feed> GetFeedById(int feedId);
        public Task<IEnumerable<Feed>> GetFeeds(QueryParameters queryParameters);
        public Task<Feed> InsertFeed(Feed feed);
    }
}
