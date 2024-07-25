using DocumentManagement.Application.Interfaces;
using DocumentManagement.Domain.Context;
using MailKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DocumentManagement.Application.DTOs;
using System.Security.Principal;
using DocumentManagement.Domain.Entities;
using System.Data;

namespace DocumentManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly MyDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        public UserService(MyDbContext dbContext, IConfiguration configuration, EmailService emailService)
        {

            _dbContext = dbContext;
            _configuration = configuration;
            _emailService = emailService;

        }

        public async Task<bool> RegisterUserAsync(RegisterUserDto userDto)
        {
            // Kiểm tra xem email đã tồn tại chưa
            if (await _dbContext.User.AnyAsync(u => u.Email == userDto.Email))
            {
                return false; // Hoặc ném ra ngoại lệ
            }

            // Hash mật khẩu
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            // Tạo người dùng mới
            var user = new Users
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Address = userDto.Address,
                Email = userDto.Email,
                Gender = userDto.Gender,
                Password = passwordHash,
                RoleId = userDto.RoleId,
                DepartmentId = userDto.DepartmentId
            };

            _dbContext.User.Add(user);
            await _dbContext.SaveChangesAsync();

            // Gửi email với mật khẩu
            await _emailService.SendEmail(new SendEmailDTOs
            {
                ToEmail = user.Email,
                Subject = "Xác nhận đăng ký tài khoản",
                Body = $"Chào mừng bạn đến với ứng dụng của chúng tôi! Tài khoản của bạn đã được đăng ký thành công."
            });

            return true;
        }

        public async Task<string> GenerateToken(Login_DTOs user)
        {
            var userId = await GetUserid(user.Email);

            // Lấy khóa bảo mật từ cấu hình
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Tạo các thông tin (claims) cho token
            var claims = new List<Claim>
            {
                new Claim("userId", userId.ToString()),  // Thêm claim cho Id của người dùng
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Tạo token với các thông tin và thời gian hết hạn
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            // Trả về token dưới dạng chuỗi
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<int> GetUserid(string email)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Email == email);
            return user.Id;
        }
        public async Task<string> Login(string email, string password)
        {
            // Tìm người dùng theo email từ cơ sở dữ liệu
            var user = await _dbContext.User.FirstOrDefaultAsync(x => x.Email == email);

            // Nếu không tìm thấy hoặc mật khẩu không đúng, trả về null
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password ))
            {
                return null;
            }

            var loginDTO = await GenerateToken(new Login_DTOs
            {
                Email = user.Email                
            });

            // Nếu tìm thấy và mật khẩu đúng, trả về người dùng
            return loginDTO;
        }

        //public Task<Users> GetUserById(int userId)
        //{
        //    var user = _dbContext.User
        //   .Include(u => u.Role)
        //   .ThenInclude(r => r.RolePermission)
        //   .FirstOrDefault(u => u.UserId == userId);
        //    return user;
        //}

        //public async Task<bool> HasPermissionAsync(int userId, string permissionName)
        //{
        //    var user = await _dbContext.User
        //   .Include(u => u.Role)
        //   .ThenInclude(r => r.RoleName)
        //   .ThenInclude(rp => rp.)
        //   .FirstOrDefaultAsync(u => u.Id == userId);

        //    if (user == null)
        //    {
        //        return false;
        //    }

        //    return user.Role.RolePermissions.Any(rp => rp.Permission.Name == permissionName);
        //}
    }
}
