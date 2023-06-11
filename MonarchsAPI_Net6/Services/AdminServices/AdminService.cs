using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MonarchsAPI_Net6.Data;
using MonarchsAPI_Net6.DTOs.AdminDtos;
using MonarchsAPI_Net6.DTOs.UserDtos;
using MonarchsAPI_Net6.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MonarchsAPI_Net6.Services.AdminServices
{
    public class AdminService : IAdminService
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _dbContext;

        public AdminService(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _dbContext = context;
        }

        public async Task<AdminLoginResponse> LoginAdmin(AdminLoginRequest requestDto)
        {
            Admin? foundAdmin = await _dbContext.Admins.Where(a => a.Username == requestDto.Username).FirstOrDefaultAsync();
            if (foundAdmin != null && VerifyPassword(requestDto.Password, foundAdmin.PasswordHash, foundAdmin.PasswordSalt))
            {
                return new AdminLoginResponse()
                {
                    Username = foundAdmin.Username,
                    Token = GenerateToken(requestDto)
                };
            }
            return null;
        }

        public async Task<bool> RegisterAdmin(AdminLoginRequest requestDto)
        {
            CreatePasswordHash(requestDto.Password, out byte[] hash, out byte[] salt);
            Admin newAdmin = new() 
            { 
                Username = requestDto.Username,
                PasswordHash = hash,
                PasswordSalt = salt
            };
            try
            {
                await _dbContext.Admins.AddAsync(newAdmin);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> DeleteAdmin(int id)
        {
            Admin adminToDelete = await FindById(id);
            if(adminToDelete != null)
            {
                try
                {
                    _dbContext.Admins.Remove(adminToDelete);
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return true;
            }
            return false;
        }

        public async Task<List<AdminGetResponseDto>> GetAllAdmins()
        {
            try
            {
                List<Admin> admins = await _dbContext.Admins.ToListAsync();
                List<AdminGetResponseDto> responseDtos = new();
                foreach (Admin admin in admins)
                {
                    responseDtos.Add(new AdminGetResponseDto() { Id = admin.Id, Username = admin.Username});
                }
                return responseDtos;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<AdminGetResponseDto> GetAdminById(int id)
        {
            try
            {
                Admin? admin = await FindById(id);
                if(admin == null)
                {
                    return null;
                }
                return new AdminGetResponseDto() { Id = admin.Id, Username= admin.Username };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private async Task<Admin> FindById(int id)
        {
            Admin? admin = await _dbContext.Admins.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (admin != null)
            {
                return admin;
            }
            return null;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private string GenerateToken(AdminLoginRequest userDto)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, userDto.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: cred
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}

