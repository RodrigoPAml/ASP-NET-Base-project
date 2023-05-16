using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Infra.Responses;
using API.Infra.Exceptions;
using API.Services.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;

        private readonly IAuthenticationService _service;

        public AuthenticationController(IServiceProvider provider, ILogger<AuthenticationController> logger)
        {
            _logger = logger;
            _service = provider.GetService<IAuthenticationService>();
        }

        /// <summary>
        /// Login and get the token for access
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        [AllowAnonymous]
        public ResponseBody Login(string login, string password)
        {
            try
            {
                var token = _service.GetToken(login, password);
                return ResponseBody.WithContentSuccess("Logged with success!", token);
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
        /// Test if the token is valid
        /// </summary>
        /// <returns></returns>
        [HttpGet("TestToken")]
        [Authorize]
        public ResponseBody Test()
        {
            try
            {
                return ResponseBody.NoContentSuccess(string.Empty);
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