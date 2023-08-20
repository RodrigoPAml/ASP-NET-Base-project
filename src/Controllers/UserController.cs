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
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        private readonly IUserService _service;

        public UserController(IServiceProvider provider, ILogger<UserController> logger)
        {
            _logger = logger;
            _service = provider.GetService<IUserService>();
        }

        /// <summary>
        /// Get Users paginated
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
        /// Get User by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetUser")]
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
        /// Create new User
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        [HttpPost("CreateUser")]
        [AllowAnonymous]
        public ResponseBody Post([FromBody] NewUser newEntity)
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
        /// Update User
        /// </summary>
        /// <param name="updatedEntity"></param>
        /// <returns></returns>
        [HttpPut("UpdateUser")]
        [Authorize]
        public ResponseBody Put([FromBody] UpdatedUser updatedEntity)
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
        /// Delete User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteUser")]
        [Authorize]
        public ResponseBody DeleteUser(ulong id)
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