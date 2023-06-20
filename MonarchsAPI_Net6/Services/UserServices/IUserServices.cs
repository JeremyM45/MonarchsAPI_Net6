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
        Task<UserLoginResponseDto> EditUser(UserEditRequestDto requestDto);
        Task<bool> DeleteUser(UserDeleteRequestDto requestDto);
        Task<UserLoginResponseDto> LoginUser(UserLoginRequestDto requestDto);
        Task<bool> VerifyUser(string name, string password);
        Task<User> GetUserByName(string name);
        Task<bool> UsernameExsits(string name);
    }
}
