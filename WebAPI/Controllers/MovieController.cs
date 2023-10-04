using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Application.Responses;
using Application.AppServices.Interfaces;
using Application.Models.NewEntity;
using Application.Models.UpdatedEntity;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly ILogger<MovieController> _logger;

        private readonly IMovieAppService _service;

        public MovieController(IServiceProvider provider, ILogger<MovieController> logger)
        {
            _logger = logger;
            _service = provider.GetService<IMovieAppService>();
        }

        /// <summary>
        /// Get Movies paginated
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="filters"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        [HttpGet("GetPaged")]
        [Authorize]
        public ResponseBody Get(uint page, uint pageSize, string? filters, string? orderBy)
        {
            return _service.GetPaged(page, pageSize, filters, orderBy);
        }

        /// <summary>
        /// Get Movie by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetMovie")]
        [Authorize]
        public ResponseBody Get(ulong id)
        {
            return _service.Get(id);
        }

        /// <summary>
        /// Create new Movie
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        [HttpPost("CreateMovie")]
        [Authorize]
        public ResponseBody Post([FromBody] NewMovie newEntity)
        {
            return _service.Create(newEntity);
        }

        /// <summary>
        /// Update Movie
        /// </summary>
        /// <param name="updatedEntity"></param>
        /// <returns></returns>
        [HttpPut("UpdateMovie")]
        [Authorize]
        public ResponseBody Put([FromBody] UpdatedMovie updatedEntity)
        {
            return _service.Update(updatedEntity);
        }

        /// <summary>
        /// Delete Movie
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteMovie")]
        [Authorize]
        public ResponseBody DeleteMovie(ulong id)
        {
            return _service.Delete(id);
        }
    }
}