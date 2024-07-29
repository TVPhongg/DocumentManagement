using DocumentManagement.Application.DTOs;
using DocumentManagement.Application.Interfaces;
using DocumentManagement.Application.Services;
using DocumentManagement.Domain.Entities;
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

        [HttpPost("login")]
        public async Task<ResponseModel> Login([FromBody] Login_DTOs login_DTOs )
        {
            try
            {
                var user = await _userService.Login(login_DTOs.Email, login_DTOs.Password);              

                var response = new ResponseModel
                {
                    statusCode = 201, 
                    message = "Đăng nhập thành công",
                    data = user
                };
                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 403,
                    message = "Đăng nhập thất bại"
                };
                return errorResponse;
            }
           
        }


        [HttpDelete]
        public async Task<ResponseModel> DeleteUser(int Id)
        {
            try
            {
                var user = await _userService.DeleteUser(Id);

                var response = new ResponseModel
                {
                    statusCode = 200,
                    message = "Xóa người dùng thành công",
                    data = user
                };
                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 403,
                    message = "Xóa người dùng thất bại"
                };
                return errorResponse;
            }
        }


        [HttpPut]
        public async Task<ResponseModel> UpdateUser(int Id, RegisterUserDto registerUserDto)
        {
            try
            {
                var user = await _userService.UpdateUser(Id, registerUserDto);

                var response = new ResponseModel
                {
                    statusCode = 200,
                    message = "Cập nhật người dùng thành công",
                    data = user
                };
                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 403,
                    message = "Cập nhật người dùng thất bại"
                };
                return errorResponse;
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

 
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 2)
        {
            var result = await _userService.GetUsersAsync(pageNumber, pageSize);

            return Ok(result);
        }

        [HttpGet("search-by-email")]
        public async Task<IActionResult> SearchUsersByEmail([FromQuery] string emailSearchTerm)
        {
            var users = await _userService.SearchUsersByEmailAsync(emailSearchTerm);

            if (!users.Any())
                return NotFound();

            return Ok(users);
        }

    }
}
