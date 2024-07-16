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
using SendGrid;
using SendGrid.Helpers.Mail;
using DocumentManagement.Application.DTOs;

namespace DocumentManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly MyDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        public UserService( MyDbContext dbContext, IConfiguration configuration,
            IEmailService emailService)
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
                RoleId = userDto.RoleId
            };

            _dbContext.User.Add(user);
            await _dbContext.SaveChangesAsync();

            // Gửi email với mật khẩu
            await _emailService.SendEmailAsync(new List<string> { user.Email }, "Đăng ký thành công", $"Mật khẩu của bạn là: {userDto.Password}");

            return true;
        }

        public async Task<string> Signin(string email, string password)
        {
            var user = await _dbContext.User.SingleOrDefaultAsync(u => u.Email == email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                // Trường hợp không xác thực thành công
                return null;
            }

            // Tạo claims (thông tin người dùng) cho JWT token
            var claims = new[]
            {
            new Claim(ClaimTypes.Email, email)
            // Các thông tin khác về người dùng có thể được thêm vào đây
        };

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);

            // Tạo JWT token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1), // Thời hạn của token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token); // Trả về JWT token dưới dạng string
        }

    }
}
