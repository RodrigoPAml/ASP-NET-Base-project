using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Application.Responses;
using Application.AppServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Infra.Authentication;
using Domain.Exceptions;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;

        private readonly IUserAppService _service;

        public AuthenticationController(IServiceProvider provider, ILogger<AuthenticationController> logger)
        {
            _logger = logger;
            _service = provider.GetService<IUserAppService>();
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
                var claims = _service.GetLogin(login, password);
                return ResponseBody.WithContentSuccess("Logged with success!", TokenGenerator.CreateToken(claims, DateTime.UtcNow.AddHours(1)));
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
            return ResponseBody.NoContentSuccess(string.Empty);
        }
    }
}