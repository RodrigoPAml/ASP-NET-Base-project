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
    public class SessionController : ControllerBase
    {
        private readonly ILogger<SessionController> _logger;

        private readonly ISessionService _service;

        public SessionController(IServiceProvider provider, ILogger<SessionController> logger)
        {
            _logger = logger;
            _service = provider.GetService<ISessionService>();
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
        /// Get Session by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetSession")]
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
        /// Create new Session
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        [HttpPost("CreateSession")]
        [AllowAnonymous]
        public ResponseBody Post([FromBody] NewSession newEntity)
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
        /// Update Session
        /// </summary>
        /// <param name="updatedEntity"></param>
        /// <returns></returns>
        [HttpPut("UpdateSession")]
        [Authorize]
        public ResponseBody Put([FromBody] UpdatedSession updatedEntity)
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
        /// Delete Session
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteSession")]
        [Authorize]
        public ResponseBody DeleteSession(ulong id)
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