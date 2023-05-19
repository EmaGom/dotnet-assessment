using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TimeChimp.Backend.Assessment.Controllers.V1;
using TimeChimp.Backend.Assessment.Managers;
using TimeChimp.Backend.Assessment.Models;

namespace TimeChimp.Backend.Assessment.UnitTests.ControllerTests.V1
{
    [TestFixture]
    public class FeedControllerTest
    {
        private Mock<IFeedsManager> _feedsManager;
        private FeedController _sut;

        [SetUp]
        public void init()
        {
            this._feedsManager = new Mock<IFeedsManager>();
            this._sut = new FeedController(this._feedsManager.Object);
        }

        [TearDown]
        public void destroy()
        {
            this._feedsManager = null;
            this._sut = null;
        }

        #region GetFeeds
        [Test]
        [Category("Get")]
        public async Task Get_Success() => await this.GenericGet(false, StatusCodes.Status200OK);

        [Test]
        [Category("Get")]
        public async Task Get_InternalServerError() => await this.GenericGet(true, StatusCodes.Status500InternalServerError);
        private async Task GenericGet(bool exceptionThrown, int expectedStatusCode)
        {
            // Arrange
            var response = new List<Feed>() {
                new Feed()
                {
                    Category = null,
                    CategoryId = 1,
                    Id = 1,
                    PublishDate = DateTime.Now,
                    Title = "some_title",
                    Url = "some_url"
                }
            };

            var mockGetFeeds = this._feedsManager.Setup(x => x.GetFeeds(It.IsAny<QueryParameters>()));

            if (exceptionThrown)
                mockGetFeeds.ThrowsAsync(new Exception());
            else
                mockGetFeeds.ReturnsAsync(response);

            // Act
            IActionResult result = await this._sut.Get(It.IsAny<QueryParameters>());

            // Assert
            this.Generic_Asserts(result, expectedStatusCode);
        }
        #endregion

        #region GetFeedsById
        [Test]
        [Category("GetById")]
        public async Task GetById_Success() => await this.GenericGetById(false, StatusCodes.Status200OK);

        [Test]
        [Category("GetById")]
        public async Task GetById_NotFound() => await this.GenericGetById(false, StatusCodes.Status404NotFound);

        [Test]
        [Category("GetById")]
        public async Task GetById_InternalServerError() => await this.GenericGetById(true, StatusCodes.Status500InternalServerError);

        private async Task GenericGetById(bool exceptionThrown, int expectedStatusCode)
        {
            // Arrange
            var response = new Feed()
            {
                Category = null,
                CategoryId = 1,
                Id = 1,
                PublishDate = DateTime.Now,
                Title = "some_title",
                Url = "some_url"
            };


            var mockGetFeedById = this._feedsManager.Setup(x => x.GetFeedById(It.IsAny<int>()));

            if (expectedStatusCode == StatusCodes.Status404NotFound)
                response = null;

            if (exceptionThrown)
                mockGetFeedById.ThrowsAsync(new Exception());
            else
                mockGetFeedById.ReturnsAsync(response);

            // Act
            IActionResult result = await this._sut.GetById(It.IsAny<int>());

            // Assert
            this.Generic_Asserts(result, expectedStatusCode);
        }
        #endregion

        #region Create
        [Test]
        [Category("Create")]
        public async Task Create_Success() => await this.GenericCreate(new CategoryRequest() { Name = "some_name" }, false, StatusCodes.Status201Created);

        [Test]
        [Category("Create")]
        public async Task Create_BadRequest() => await this.GenericCreate(new CategoryRequest(), false, StatusCodes.Status400BadRequest);

        [Test]
        [Category("Create")]
        public async Task Create_InternalServerError() => await this.GenericCreate(new CategoryRequest() { Name = "some_name" }, true, StatusCodes.Status500InternalServerError);

        private async Task GenericCreate(CategoryRequest categoryRequest, bool exceptionThrown, int expectedStatusCode)
        {
            // Arrange
            var response = new Category()
            {
                Id = 1,
                LastBuildDate = DateTime.Now,
                Name = "some_category",
                Feeds = new List<Feed>()
                {
                    new Feed()
                    {
                        Category = null,
                        CategoryId = 1,
                        Id = 1,
                        PublishDate = DateTime.Now,
                        Title = "some_title",
                        Url = "some_url"
                    }
                }
            };


            var mockCreate = this._feedsManager.Setup(x => x.InsertFeeds(It.IsAny<string>()));

            if (exceptionThrown)
                mockCreate.ThrowsAsync(new Exception());
            else
                mockCreate.ReturnsAsync(response);

            // Act
            IActionResult result = await this._sut.Create(categoryRequest);

            // Assert
            this.Generic_Asserts(result, expectedStatusCode);
        }
        #endregion

        protected void Generic_Asserts(IActionResult result, int expectedStatusCode)
        {
            // Assert
            switch (expectedStatusCode)
            {
                case 200:
                    result.Should().BeOfType<OkObjectResult>();
                    result.As<OkObjectResult>().StatusCode.Should().Be(expectedStatusCode);
                    break;
                case 201:
                    result.Should().BeOfType<ObjectResult>();
                    result.As<ObjectResult>().StatusCode.Should().Be(expectedStatusCode);
                    break;
                case 400:
                    result.Should().BeOfType<BadRequestResult>();
                    result.As<BadRequestResult>().StatusCode.Should().Be(expectedStatusCode);
                    break;
                case 404:
                    result.Should().BeOfType<NotFoundResult>();
                    result.As<NotFoundResult>().StatusCode.Should().Be(expectedStatusCode);
                    break;
                case 500:
                    result.As<ObjectResult>().StatusCode.Should().Be(expectedStatusCode);
                    break;
                default:
                    Assert.Fail($"Expected {expectedStatusCode}");
                    break;
            }
        }
    }
}
