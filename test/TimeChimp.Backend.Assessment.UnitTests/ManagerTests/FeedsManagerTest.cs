using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeChimp.Backend.Assessment.Enums;
using TimeChimp.Backend.Assessment.Helpers;
using TimeChimp.Backend.Assessment.Managers;
using TimeChimp.Backend.Assessment.Models;
using TimeChimp.Backend.Assessment.Repositories;

namespace TimeChimp.Backend.Assessment.UnitTests.ManagerTests
{
    [TestFixture]
    public class FeedsManagerTest
    {
        private Mock<IReaderService> _readerService;
        private Mock<ICacheService> _cacheService;
        private Mock<IDataAccessLayerFactory> _dataAccessLayerFactory;
        private Mock<IDataAccessLayer> _dataAccessLayer;
        private Mock<ILogger<FeedsManager>> _logger;
        private FeedsManager _sut;

        [SetUp]
        public void Init()
        {
            this._readerService = new Mock<IReaderService>();
            this._cacheService = new Mock<ICacheService>(); 
            this._dataAccessLayerFactory = new Mock<IDataAccessLayerFactory>();
            this._dataAccessLayer = new Mock<IDataAccessLayer>();
            this._logger = new Mock<ILogger<FeedsManager>>();
            this._dataAccessLayerFactory.Setup(x => x.GetInstance(It.IsAny<DataAccessLayerEnum>())).Returns(this._dataAccessLayer.Object);

            this._sut = new FeedsManager(this._readerService.Object, this._dataAccessLayerFactory.Object, this._cacheService.Object, this._logger.Object);
        }

        [TearDown]
        public void destroy()
        {
            this._readerService = null;
            this._cacheService = null;
            this._dataAccessLayerFactory = null;
            this._logger = null;
            this._sut = null;
        }

        [Test]
        public async Task GetFeeds()
        {
            IEnumerable<Feed> feeds = new List<Feed>() { new Feed() };
            this.SetLogExceptionIfFail(feeds);

            this._dataAccessLayer.Setup(d => d.GetFeeds(It.IsAny<QueryParameters>())).ReturnsAsync(feeds);

            // Act
            var result = await this._sut.GetFeeds(It.IsAny<QueryParameters>());

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<Feed>>();
            result.Should().HaveCount(feeds.ToList().Count());
            result.Should().BeEquivalentTo(feeds);
        }

        [Test]
        public async Task GetById()
        {
            Feed feed = new Feed() { Id = 123 };
            this.SetLogExceptionIfFail(feed);

            this._dataAccessLayer.Setup(d => d.GetFeedById(It.IsAny<int>())).ReturnsAsync(feed);

            // Act
            var result = await this._sut.GetFeedById(It.IsAny<int>());

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Feed>();
            result.Should().BeEquivalentTo(feed);
        }

        [Test]
        public async Task InsertFeeds()
        {
            var feeds = new List<Feed>()
            {
                new Feed()
                {
                    Id = 1
                }
            };
            var category = new Category()
            {
                Id = 1,
                Name = "some_category",
                Feeds = feeds
            };

            this.SeTLogExceptionAndRollbackTransactionIfFail(category);

            this._dataAccessLayer.Setup(d => d.InsertCategory(It.IsAny<Category>())).ReturnsAsync(category);
            this._dataAccessLayer.Setup(d => d.InsertFeed(It.IsAny<Feed>())).ReturnsAsync(feeds.First());

            // Act
            var result = await this._sut.InsertFeeds(It.IsAny<string>());

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Category>();
            result.Should().BeEquivalentTo(category);
        }
        private void SetLogExceptionIfFail<T>(T returnValue)
        {
            // Setup
            this._dataAccessLayer.Setup(d => d.LogExceptionIfFail(
                It.IsAny<ILogger>(),
                It.IsAny<Func<Task<T>>>()))
                .Returns((ILogger aLogger, Func<Task<T>> func) =>
                new DataAccessLayerDapper(new Mock<IContext>().Object)
                    .LogExceptionIfFail(aLogger, func));
        }
        private void SeTLogExceptionAndRollbackTransactionIfFail<T>(T returnValue)
        {
            // Setup
            this._dataAccessLayer.Setup(d => d.LogExceptionAndRollbackTransactionIfFail(
                It.IsAny<ILogger>(),
                It.IsAny<Func<Task<T>>>()))
                .Returns((ILogger aLogger, Func<Task<T>> func) =>
                new DataAccessLayerDapper(new Mock<IContext>().Object)
                    .LogExceptionAndRollbackTransactionIfFail(aLogger, func));
        }
    }
}
