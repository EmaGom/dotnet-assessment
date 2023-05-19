using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TimeChimp.Backend.Assessment.Managers;
using TimeChimp.Backend.Assessment.Models;

namespace TimeChimp.Backend.Assessment.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [AllowAnonymous]
    public class FeedController : Controller
    {
        private readonly IFeedsManager _feedsManager;

        public FeedController(IFeedsManager feedsManager)
        {
            this._feedsManager = feedsManager;
        }

        /// <summary>
        /// Get Feeds by query parameters
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] QueryParameters queryParemeters)
        {
            try
            {
                var feedsResult = await _feedsManager.GetFeeds(queryParemeters);
                return Ok(feedsResult);
            } 
            catch(Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Get a Feed by Id.
        /// </summary>
        /// <returns>A feed</returns>
        /// <response code="200">Returns the item</response>
        /// <response code="400">If the item is null or invalid</response>
        /// <response code="500">If errors in the process</response>
        [HttpGet("{id}", Name = nameof(GetById))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var feedResult = await this._feedsManager.GetFeedById(id);
                
                if(feedResult != null)
                    return Ok(feedResult);

                return NotFound();
            }
            catch(Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }


        /// <summary>
        /// Save a new Feed
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /v1/feed
        ///     {
        ///        "Id": "40f96542-ac26-45c3-8fe1-a7b8c8f97d08", NOT Required.
        ///        "Title": "This is an example feed",
        ///        "PostedDateTime": "2023-05-17",
        ///        "Url": "https://.."
        ///     }
        /// </remarks>
        /// <returns>A newly created feed</returns>
        /// <response code="201">Returns the newly created feed</response>
        /// <response code="400">If the feed is null or invalid</response>
        /// <response code="500">If errors in the process</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Create(Feed feed)
        {
            try
            {
                var feedResult = await this._feedsManager.InsertFeed(feed);
                if (feedResult != null)
                {
                    return StatusCode(StatusCodes.Status201Created, feedResult);
                }
                
                return BadRequest();
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }
    }
}