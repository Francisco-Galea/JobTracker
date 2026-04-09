using JobTracker.Application.Common.Exceptions;
using JobTracker.Application.UseCases.LoginUser;
using JobTracker.Application.UseCases.RegisterUser;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly RegisterUserHandler registerHandler;
        private readonly LoginUserHandler loginHandler;

        public AuthController(
            RegisterUserHandler registerHandler,
            LoginUserHandler loginHandler)
        {
            this.registerHandler = registerHandler;
            this.loginHandler = loginHandler;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterUserCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await registerHandler.HandleAsync(command, cancellationToken);
                return Ok(result);
            }
            catch (ConflictException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginUserCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await loginHandler.HandleAsync(command, cancellationToken);
                return Ok(result);
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
