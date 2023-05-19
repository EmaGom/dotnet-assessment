using Microsoft.Extensions.Options;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Xml;
using TimeChimp.Backend.Assessment.Models;

namespace TimeChimp.Backend.Assessment.Helpers
{
    public class ReaderService : IReaderService
    {
        private readonly FeedsConfiguration _feedsConfiguration;
        public ReaderService(IOptions<FeedsConfiguration> feedsConfiguration) {
            _feedsConfiguration = feedsConfiguration.Value;
        }
        public Category Read(string categoryName)
        {
            var url = $"{_feedsConfiguration.Url}{categoryName}";
            
            IList<Feed> feeds = new List<Feed>();

            using (var reader = XmlReader.Create(url))
            {
                var categoryFeed = SyndicationFeed.Load(reader);
                foreach (var feed in categoryFeed.Items)
                {
                    feeds.Add(new()
                    {
                        PublishDate = feed.PublishDate.DateTime,
                        Title = feed.Title.Text,
                        Url = feed.Id.Replace("-", categoryName),
                    });
                }

                return new Category()
                {
                    Name = categoryFeed.Title.Text,
                    LastBuildDate = categoryFeed.LastUpdatedTime.DateTime,
                    Feeds = feeds
                };
            }
        }
    }
}
