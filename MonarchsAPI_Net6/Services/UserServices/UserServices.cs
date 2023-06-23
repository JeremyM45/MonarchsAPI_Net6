using AutoMapper;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UserServices(DataContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _dbContext = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        
        public async Task<List<UserGetResponseDto>> GetAllUsers()
        {
            List<User> users = await _dbContext.Users.Include(u => u.Ratings).ToListAsync();
            List<UserGetResponseDto> userDtos = users.Select(u => _mapper.Map<UserGetResponseDto>(u)).ToList();
            
            return userDtos;
        }

        public async Task<UserGetResponseDto> GetUserById(int id)
        {
            User? user = await _dbContext.Users.FindAsync(id);
            if (user == null) { return null; }
            UserGetResponseDto userDto = _mapper.Map<UserGetResponseDto>(user);
            return userDto;
        }
        public async Task<User> GetUserByName(string name)
        {
            User? foundUser = await _dbContext.Users.Where(u => u.UserName == name).FirstOrDefaultAsync();
            if(foundUser == null)
            {
                return null;
            }
            return foundUser;
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

        public async Task<bool> DeleteUser(UserDeleteRequestDto requestDto)
        {
            User? userToRemove = await _dbContext.Users.FindAsync(requestDto.Id);
            if(userToRemove != null && (await VerifyUser(userToRemove.UserName, requestDto.Password) || IsAdmin()))
            {
                _dbContext.Users.Remove(userToRemove);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<UserLoginResponseDto> EditUser(UserEditRequestDto requestDto)
        {
            User? userToEdit = await _dbContext.Users.Where(u => u.UserName == requestDto.Username).FirstOrDefaultAsync();
            if (userToEdit != null && await VerifyUser(userToEdit.UserName, requestDto.Password)) 
            {
                userToEdit.UserName = requestDto.NewUsername;
                userToEdit.UserEmail = requestDto.NewEmail;
                CreatePasswordHash(requestDto.NewPassword, out byte[] newHash, out byte[] newSalt);
                userToEdit.PasswordHash = newHash;
                userToEdit.PasswordSalt = newSalt;
                await _dbContext.SaveChangesAsync();

                UserLoginResponseDto responseDto = new()
                {
                    Id = userToEdit.Id,
                    UserName = userToEdit.UserName,
                    Token = GenerateToken(userToEdit.UserName),
                };
                return responseDto;
            }
            return null;
            
        }
        public async Task<bool> VerifyUser(string name, string password)
        {
            if(_httpContextAccessor != null)
            {
                User? foundUser = await _dbContext.Users.Where(u => u.UserName == name).FirstOrDefaultAsync();
                if (foundUser != null) 
                {
                    return VerifyJwt(name) && VerifyPassword(password, foundUser.PasswordHash, foundUser.PasswordSalt);
                }
            }
            return false;
        }

        public async Task<UserLoginResponseDto> LoginUser(UserLoginRequestDto loginDto)
        {
            User? foundUser = await _dbContext.Users.Where(u => u.UserName == loginDto.UserName).FirstOrDefaultAsync();
            if(foundUser != null)
            {
                UserLoginResponseDto responseDto = new()
                {
                    Id = foundUser.Id,
                    UserName = foundUser.UserName,
                    Token = GenerateToken(loginDto.UserName),
                };
                return responseDto;
            }
            return null;
        }
        public async Task<bool> UsernameExsits(string name)
        {
            User? foundUser = await GetUserByName(name);
            if(foundUser == null)
            {
                return false;
            }
            return true;

        }

        private bool VerifyJwt(string username)
        {
            return username == _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
        }
        private bool IsAdmin()
        {
            return _httpContextAccessor.HttpContext.User.IsInRole("Admin");
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        public bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private string GenerateToken(string username)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, username),
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
