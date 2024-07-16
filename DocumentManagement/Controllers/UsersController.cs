using DocumentManagement.Application.DTOs;
using DocumentManagement.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            var result = await _userService.RegisterUserAsync(registerUserDto);
            if (!result)
            {
                return BadRequest("Email đã tồn tại.");
            }
            return Ok("Đăng ký thành công.");
        }
    }
}
