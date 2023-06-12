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
        public async Task<ActionResult<List<User>>> GetAll()
        {
            List<User> users = await _userServices.GetAllUsers();
            if(users == null) { return NotFound(); }
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            User user = await _userServices.GetUserById(id);
            if(user == null) { return NotFound(); }
            return Ok(user);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<UserLoginResponseDto>> LoginUser([FromQuery] UserLoginRequestDto userLoginRequest)
        {
            UserLoginResponseDto? user = await _userServices.LoginUser(userLoginRequest);
            if (user == null) { return NotFound(); };
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult> AddUser(CreateUserDto request)
        {
            
            if(await _userServices.AddUser(request))
            {
                return Ok();
            }
            return BadRequest();   
        }

        [HttpDelete, Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            if(await _userServices.DeleteUser(id))
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpPut, Authorize(Roles = "User")]
        public async Task<ActionResult> EditUser(UserEditRequestDto requestDto)
        {
            if(await _userServices.VerifyUser(requestDto.Username, requestDto.Password))
            {
                UserEditResponseDto? responseDto = await _userServices.EditUser(requestDto);
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