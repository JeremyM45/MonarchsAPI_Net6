using Microsoft.AspNetCore.Mvc;
using MonarchsAPI_Net6.DTOs.UserDtos;
using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.Services.UserServices
{
    public interface IUserServices
    {
        Task<List<UserGetResponseDto>> GetAllUsers();
        Task<UserGetResponseDto> GetUserById(int id);
        Task<bool> AddUser(CreateUserDto newUserDto);
        Task<UserLoginResponseDto> EditUser(UserEditRequestDto userDto);
        Task<bool> DeleteUser(int id);
        Task<UserLoginResponseDto> LoginUser(UserLoginRequestDto loginDto);
        Task<bool> VerifyUser(string name, string password);
        Task<User> GetUserByName(string name);
    }
}
