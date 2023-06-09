﻿using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonarchsAPI_Net6.Data;
using MonarchsAPI_Net6.DTOs.UserDtos;
using MonarchsAPI_Net6.Models;
using MonarchsAPI_Net6.Services.UserServices;

namespace MonarchsAPI_Net6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }



        [HttpGet]
        public async Task<ActionResult<List<UserGetResponseDto>>> GetAll()
        {
            List<UserGetResponseDto> users = await _userServices.GetAllUsers();
            if (users == null) { return NotFound(); }
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            UserGetResponseDto user = await _userServices.GetUserById(id);
            if(user == null) { return NotFound(); }
            return Ok(user);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<UserLoginResponseDto>> LoginUser([FromQuery] UserLoginRequestDto userLoginRequest)
        {
            if(!await _userServices.UsernameExsits(userLoginRequest.UserName))
            {
                return NotFound("Username Does Not Exisit");
            }
            User? foundUser = await _userServices.GetUserByName(userLoginRequest.UserName);
            if(!_userServices.VerifyPassword(userLoginRequest.Password, foundUser.PasswordHash, foundUser.PasswordSalt))
            {
                return BadRequest("Invalid Password");
            }
            UserLoginResponseDto? user = await _userServices.LoginUser(userLoginRequest);
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult> AddUser(CreateUserDto request)
        {
            if(await _userServices.UsernameExsits(request.UserName))
            {
                return BadRequest("Username Already Exsits");
            }
            if(await _userServices.AddUser(request))
            {
                return Ok();
            }
            return BadRequest();   
        }

        [HttpDelete, Authorize(Roles = "User, Admin")]
        public async Task<ActionResult> DeleteUser(UserDeleteRequestDto requestDto)
        {
            if(await _userServices.DeleteUser(requestDto))
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpPut, Authorize(Roles = "User, Admin")]
        public async Task<ActionResult> EditUser(UserEditRequestDto requestDto)
        {
            if(await _userServices.VerifyUser(requestDto.Username, requestDto.Password))
            {
                if (requestDto.NewUsername != requestDto.Username && await _userServices.UsernameExsits(requestDto.NewUsername))
                {
                    return BadRequest("Can't Change Username, Username Already Exsits");
                }
                UserLoginResponseDto? responseDto = await _userServices.EditUser(requestDto);
                if (responseDto != null)
                {
                    return Ok(responseDto);
                }
                return BadRequest();
            }
            return Forbid();
            
        }
    }
}