﻿using Microsoft.AspNetCore.Mvc;
using MonarchsAPI_Net6.DTOs.UserDtos;
using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.Services.UserServices
{
    public interface IUserServices
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<bool> AddUser(CreateUserDto newUserDto);
        Task<bool> EditUser(User user);
        Task<bool> DeleteUser(int id);
        Task<UserLoginResponseDto> LoginUser(UserLoginRequestDto loginDto);
    }
}
