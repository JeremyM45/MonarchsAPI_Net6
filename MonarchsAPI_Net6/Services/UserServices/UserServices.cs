using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MonarchsAPI_Net6.Data;
using MonarchsAPI_Net6.DTOs.UserDtos;
using MonarchsAPI_Net6.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MonarchsAPI_Net6.Services.UserServices
{
    public class UserServices : IUserServices
    {
        private readonly DataContext _dbContext;
        private readonly IConfiguration _configuration;

        public UserServices(DataContext context, IConfiguration configuration)
        {
            _dbContext = context;
            _configuration = configuration;
        }
        
        public async Task<List<User>> GetAllUsers()
        {
            return await _dbContext.Users.Include(u => u.Ratings).ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            User? user = await _dbContext.Users.FindAsync(id);
            if (user == null) { return null; }
            return user;
        }

        public async Task<bool> AddUser(CreateUserDto newUserDto)
        {
            CreatePasswordHash(newUserDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            User newUser = new()
            {
                UserName = newUserDto.UserName,
                UserEmail = newUserDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            try
            {
                await _dbContext.Users.AddAsync(newUser);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw new Exception("Error: Could Not Add User");
            }
        }

        public async Task<bool> DeleteUser(int id)
        {
            User? userToRemove = await _dbContext.Users.FindAsync(id);
            if(userToRemove == null)
            {
                return false;
            }
            _dbContext.Users.Remove(userToRemove);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditUser(User user)
        {
            User? userToEdit = await _dbContext.Users.FindAsync(user.Id);
            if (userToEdit == null) { return false; }
            
            userToEdit.UserName = user.UserName;
            userToEdit.UserEmail = user.UserEmail;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<UserLoginResponseDto> LoginUser(UserLoginRequestDto loginDto)
        {
            User? foundUser = await _dbContext.Users.Where(u => u.UserName == loginDto.UserName).FirstOrDefaultAsync();
            if(foundUser != null && VerifyPassword(loginDto.Password, foundUser.PasswordHash, foundUser.PasswordSalt))
            {
                UserLoginResponseDto responseDto = new()
                {
                    Id = foundUser.Id,
                    UserName = foundUser.UserName,
                    UserEmail = foundUser.UserEmail,
                    Token = GenerateToken(loginDto),
                    Ratings = foundUser.Ratings
                };
                return responseDto;
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
        private string GenerateToken(UserLoginRequestDto userDto)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, userDto.UserName),
                new Claim(ClaimTypes.Role, "User")
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
