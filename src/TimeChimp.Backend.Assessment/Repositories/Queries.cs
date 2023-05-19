using TimeChimp.Backend.Assessment.Repositories.QueryScript;

namespace TimeChimp.Backend.Assessment.Repositories
{
    public static class Queries
    {
        public static string GetFeeds = EmbeddedResourceHelper.GetQuery("GetFeeds.sql");
        public static string InsertFeed = EmbeddedResourceHelper.GetQuery("InsertFeed.sql");
        public static string GetFeedById = EmbeddedResourceHelper.GetQuery("GetFeedById.sql");
        public static string InsertCategory = EmbeddedResourceHelper.GetQuery("InsertCategory.sql");
    }
}
