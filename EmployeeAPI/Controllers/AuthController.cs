using EmployeeAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static EmployeeAPI.DTOs.AuthDtos;

namespace EmployeeAPI.Controllers
{

    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {

        private readonly IAuthService _auth;


        public AuthController(IAuthService authService)
        {
            _auth = authService;
        }


        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        {
            var result = await _auth.LoginAsync(dto);
            if (result is null) return Unauthorized(new { message = "Invalid username or password." });
            return Ok(result);
        }

    }


}
