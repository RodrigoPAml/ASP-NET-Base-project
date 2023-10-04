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
    public class SessionController : ControllerBase
    {
        private readonly ILogger<SessionController> _logger;

        private readonly ISessionAppService _service;

        public SessionController(IServiceProvider provider, ILogger<SessionController> logger)
        {
            _logger = logger;
            _service = provider.GetService<ISessionAppService>();
        }

        /// <summary>
        /// Get Sessions paginated
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
        /// Get Session by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetSession")]
        [Authorize]
        public ResponseBody Get(ulong id)
        {
            return _service.Get(id);
        }

        /// <summary>
        /// Create new Session
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        [HttpPost("CreateSession")]
        [Authorize]
        public ResponseBody Post([FromBody] NewSession newEntity)
        {
            return _service.Create(newEntity);
        }

        /// <summary>
        /// Update Session
        /// </summary>
        /// <param name="updatedEntity"></param>
        /// <returns></returns>
        [HttpPut("UpdateSession")]
        [Authorize]
        public ResponseBody Put([FromBody] UpdatedSession updatedEntity)
        {
            return _service.Update(updatedEntity);
        }

        /// <summary>
        /// Delete Session
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteSession")]
        [Authorize]
        public ResponseBody DeleteSession(ulong id)
        {
            return _service.Delete(id);
        }
    }
}