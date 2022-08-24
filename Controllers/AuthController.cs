using ContactsApi.Dtos;
using ContactsApi.Exceptions;
using ContactsApi.Models;
using ContactsApi.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ContactsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(UserRegisterDto newUser)
        {
            var user = new User() { Username = newUser.Username };
            (bool res, string msg) = await authService.RegisterAsync(user, newUser.Password);

            return res ? BadRequest(new NotFoundException(msg)) : Ok(new { StatusCode = 200, Mensage = msg });
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLoginDto user)
        {
            (bool res, string msg) = await authService.LoginAsync(user.Username, user.Password);

            return res ? Ok(new { res, msg }) : BadRequest(new NotFoundException(msg));
        }
    }
}
