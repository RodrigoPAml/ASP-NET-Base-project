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
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        private readonly IUserAppService _service;

        public UserController(IServiceProvider provider, ILogger<UserController> logger)
        {
            _logger = logger;
            _service = provider.GetService<IUserAppService>();
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
            return _service.GetPaged(page, pageSize, filters, orderBy);
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
            return _service.Get(id);
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
            return _service.Create(newEntity);
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
            return _service.Update(updatedEntity);
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
            return _service.Delete(id);
        }
    }
}