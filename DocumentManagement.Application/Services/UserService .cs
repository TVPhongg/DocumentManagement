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
using System.Drawing.Printing;

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


        /// <summary>
        /// Xóa người dùng
        /// </summary>
        /// <param name="Id">Điền Id người dùng cần xóa</param>
        /// <returns>true:  xóa người dùng thành công
        ///          false: xóa người dùng thất bại 
        /// </returns>
        public async Task<bool> DeleteUser(int Id)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(x => x.Id == Id);
            if (user == null)
            {
                return false;
            }

            _dbContext.User.Remove(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Cập nhật người dùng 
        /// </summary>
        /// <param name="Id">Điền Id cần cập nhật</param>
        /// <param name="userDto">Điền các trường của obj cần cập nhật</param>
        /// <returns>true: cập nhật thành công
        ///          false: cập nhật thất bại 
        /// </returns>
        public async Task<bool> UpdateUser(int Id, RegisterUserDto userDto)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(x => x.Id == Id);
            if (user == null)
            {
                return false;
            }

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email;
            user.Address = userDto.Address;
            user.Gender = userDto.Gender;

            _dbContext.User.Update(user);
            await _dbContext.SaveChangesAsync();
            return true;

        }

        /// <summary>
        /// Lấy người dùng theo Id
        /// </summary>
        /// <param name="id">Điền Id người dùng </param>
        /// <returns>true: Lấy Id người dùng thành công
        ///          false: Lấy Id người dùng thất bại
        /// </returns>
        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _dbContext.User
                .Include(u => u.Role)
                .Include(u => u.Department)
                .SingleOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return null;

            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                Email = user.Email,
                Gender = user.Gender,
                RoleName = user.Role.RoleName,
                DepartmentName = user.Department.Name
            };
        }

        /// <summary>
        /// Tìm kiếm người dùng theo email
        /// </summary>
        /// <param name="emailSearchTerm">Điền ký tự, cụm ký tự cần tìm kiếm</param>
        /// <returns>true: trả về danh sách người dùng có ký tự đó
        ///          flase: trả về người dùng thất bại và báo lỗi
        /// </returns>
        public async Task<IEnumerable<UserDto>> SearchUsersByEmailAsync(string emailSearchTerm)
        {
            var users = await _dbContext.User
           .Include(u => u.Role)
           .Include(u => u.Department)
           .Where(u => u.Email.Contains(emailSearchTerm))
           .Select(user => new UserDto
           {
               Id = user.Id,
               FirstName = user.FirstName,
               LastName = user.LastName,
               Address = user.Address,
               Email = user.Email,
               Gender = user.Gender,
               RoleName = user.Role.RoleName,
               DepartmentName = user.Department.Name
           })
           .ToListAsync();
            return users;
        }

        public async Task<List<UserDto>> GetUsersAsync()
        {
            var query = _dbContext.User
                .Include(u => u.Role)
                .Include(u => u.Department);

            // Lấy danh sách người dùng
            var users = await query
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Address = user.Address,
                    Email = user.Email,
                    Gender = user.Gender,
                    RoleName = user.Role.RoleName,
                    DepartmentName = user.Department.Name
                })
                .ToListAsync();
            return users;
        }
    }
}
