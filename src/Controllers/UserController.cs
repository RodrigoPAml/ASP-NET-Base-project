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
        /// Get users paginated
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="filters"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        [HttpGet("GetPaged")]
       // [Authorize]
        public ResponseBody Get(uint page, uint pageSize, string? filters, string? orderBy)
        {
            try
            {
                var list = _service.GetPaged(page, pageSize, UserFilter.Interpret(filters), UserOrderBy.Interpret(orderBy));

                return ResponseBody.WithContentSuccess("Registros recuperados com sucesso!", list);
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
        /// Get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetUser")]
        [Authorize]
        public ResponseBody Get(ulong id)
        {
            try
            {
                var user = _service.GetUser(id);

                return ResponseBody.WithContentSuccess("Registro recuperado com sucesso!", user);
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
        /// Create new user
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        [HttpPost("CreateUser")]
        [AllowAnonymous]
        public ResponseBody Post([FromBody] NewUser newUser)
        {
            try
            {
                _service.CreateUser(newUser);

                return ResponseBody.NoContentSuccess("Registro criado com sucesso");
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
        /// Update user
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        [HttpPut("UpdateUser")]
        [Authorize]
        public ResponseBody Put([FromBody] UpdatedUser updatedUser)
        {
            try
            {
                _service.UpdateUser(updatedUser);

                return ResponseBody.NoContentSuccess("Registro atualizado com sucesso");
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
        /// Delete user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteUser")]
        [Authorize]
        public ResponseBody DeleteUser(ulong id)
        {
            try
            {
                _service.DeleteUser(id);

                return ResponseBody.NoContentSuccess("Registro deletado com sucesso");
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