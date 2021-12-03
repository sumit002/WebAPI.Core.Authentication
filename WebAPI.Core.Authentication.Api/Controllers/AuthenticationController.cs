using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Core.Authentication.Api.Models;
using WebAPI.Core.Authentication.Api.Services;

namespace WebAPI.Core.Authentication.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private ILogger _logger;
        private IAuthenticateService _authenticateService;

        public AuthenticationController(ILogger<AuthenticationController> logger, IAuthenticateService authenticateService)
        {
            _logger = logger;
            _authenticateService = authenticateService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User model, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _authenticateService.AuthenticateAsync(model.UserName, model.Password, cancellationToken);

                if (user == null)
                    return BadRequest(new { message = "Incorrect Username or Password!" });

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while Authentication!");
                return null;
            }
        }
    }
}
