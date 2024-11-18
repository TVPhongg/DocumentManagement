using DocumentManagement.Application.DTOs;
using DocumentManagement.Application.Interfaces;
using DocumentManagement.Application.Services;
using DocumentManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

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
        public async Task<ResponseModel> Register([FromBody] RegisterUserDto registerUserDto)
        {
            try
            {
                var result = await _userService.RegisterUserAsync(registerUserDto);

                var response = new ResponseModel
                {
                    statusCode = 201,
                    message = "Đăng k thành công",
                    data = result
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


        [HttpDelete("{id}")]
        public async Task<ResponseModel> DeleteUser(int id)
        {
            try
            {
                var userDeleted = await _userService.DeleteUser(id);

                if (!userDeleted)
                {
                    return new ResponseModel
                    {
                        statusCode = 404,
                        message = "Người dùng không tồn tại"
                    };
                }

                return new ResponseModel
                {
                    statusCode = 204,
                    message = "Xóa người dùng thành công"
                };
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần thiết
                Console.WriteLine(ex.Message);

                return new ResponseModel
                {
                    statusCode = 500,
                    message = "Xóa người dùng thất bại"
                };
            }
        }



        [HttpPut("{id}")]
        public async Task<ResponseModel> UpdateUser(int id, RegisterUserDto registerUserDto)
        {
            try
            {
                var user = await _userService.UpdateUser(id, registerUserDto);

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
        public async Task<ResponseModel> GetUserById(int id)
        {
            try
            {
                var result = await _userService.GetUserByIdAsync(id);

                var response = new ResponseModel
                {
                    statusCode = 200,
                    message = "Thành công.",
                    data = result
                };
                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 400,
                    data = "Null",
                };
                return errorResponse;
            }
        }

 
        [HttpGet]
        public async Task<ResponseModel> GetUsers()
        {
            try
            {
                var result = await _userService.GetUsersAsync();

                var response = new ResponseModel
                {
                    statusCode = 200,
                    message = "Thành công.",
                    data = result
                };
                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 400,
                    data = "Null",
                };
                return errorResponse;
            }
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
