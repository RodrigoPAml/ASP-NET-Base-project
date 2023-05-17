using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Infra.Responses;
using API.Infra.Exceptions;
using API.Models.NewEntity;
using API.Models.UpdatedEntity;
using API.Services.Interfaces;
using API.Infra.Query;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly ILogger<MovieController> _logger;

        private readonly IMovieService _service;

        public MovieController(IServiceProvider provider, ILogger<MovieController> logger)
        {
            _logger = logger;
            _service = provider.GetService<IMovieService>();
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
            try
            {
                var list = _service.GetPaged(page, pageSize, UserFilter.Interpret(filters), UserOrderBy.Interpret(orderBy));

                return ResponseBody.WithContentSuccess("Records retrieved successfully", list);
            }
            catch (BusinessException be)
            {
                return ResponseBody.HandledError(be);
            }
            catch (Exception e)
            {
                return ResponseBody.UnhandledError(e);
            }
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
            try
            {
                var user = _service.Get(id);

                return ResponseBody.WithContentSuccess("Record retrieved successfully", user);
            }
            catch (BusinessException be)
            {
                return ResponseBody.HandledError(be);
            }
            catch (Exception e)
            {
                return ResponseBody.UnhandledError(e);
            }
        }

        /// <summary>
        /// Create new Movie
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        [HttpPost("CreateMovie")]
        [AllowAnonymous]
        public ResponseBody Post([FromBody] NewMovie newEntity)
        {
            try
            {
                var id = _service.Create(newEntity);

                return ResponseBody.WithContentSuccess("Record created successfully", id);
            }
            catch (BusinessException be)
            {
                return ResponseBody.HandledError(be);
            }
            catch (Exception e)
            {
                return ResponseBody.UnhandledError(e);
            }
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
            try
            {
                _service.Update(updatedEntity);

                return ResponseBody.NoContentSuccess("Record updated successfully");
            }
            catch (BusinessException be)
            {
                return ResponseBody.HandledError(be);
            }
            catch (Exception e)
            {
                return ResponseBody.UnhandledError(e);
            }
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
            try
            {
                _service.Delete(id);

                return ResponseBody.NoContentSuccess("Record deleted successfully");
            }
            catch (BusinessException be)
            {
                return ResponseBody.HandledError(be);
            }
            catch (Exception e)
            {
                return ResponseBody.UnhandledError(e);
            }
        }
    }
}