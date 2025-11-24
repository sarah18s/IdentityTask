using IdentityTask.Application.Services.Interface;
using IdentityTask.Domain;
using IdentityTask.Domain.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace IdentityTask.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        public AuthController(UserManager<User> userManager , IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;

        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            try
            {
                Result result = new();
                var newUser = new User() { Email = registerRequest.Email, UserName = registerRequest.Email };
                var user = await _userManager.CreateAsync(newUser, registerRequest.Password);
                if (!user.Succeeded) {
                    result.Code = 400;
                    result.Message = "failed";
                }

                result.Code = 200;
                result.Message = "Success";
                return Ok();
            }
            catch (Exception ex) {
                return NotFound();
            }

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null) { return Unauthorized(); }
            var password = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (!password) { return Unauthorized(); }
            var createToken = _jwtTokenService.CreateToken(user , []);
            return Ok(new Result() { Code = 200, Message = createToken }); ;
        }

    }
}
