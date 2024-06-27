using BootcampSecurity.MS_Auth.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BootcampSecurity.MS_Auth.Controllers
{
    public class AuthController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly AuthService _authService;

        public AuthController(ILogger<AuthController> logger, AuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [Function("Login")]
        public async Task<IActionResult> Login([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {

            var res = await _authService.GenerateToken(req);
            return res.IsSuccess ? new OkObjectResult(res) : new BadRequestObjectResult(res);
        }

        [Function("ValidateToken")]
        public async Task<IActionResult> ValidateToken([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {

            var res = await _authService.ValidateToken(req);
            return res.IsSuccess ? new OkObjectResult(res) : new BadRequestObjectResult(res);
        }

        [Function("Refresh")]
        public async Task<IActionResult> Refresh([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            var res = await _authService.RefreshToken(req);
            return res.IsSuccess ? new OkObjectResult(res) : new BadRequestObjectResult(res);
        }

        //[Function("Ejemplo")]
        //public async Task<IActionResult> Ejemplo([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        //{
        //    var res = await _authService.ejemplo(req);
        //    return res.IsSuccess ? new OkObjectResult(res) : new BadRequestObjectResult(res);
        //}
    }
}
