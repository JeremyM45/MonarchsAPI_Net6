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

        [HttpPost]
        public async Task<ActionResult> AddUser(CreateUserDto request)
        {
            User newUser = new User
            {
                UserName = request.UserName,
                UserEmail = request.Email,
                Password = request.Password
            };

            if(await _userServices.AddUser(newUser))
            {
                return CreatedAtAction(nameof(AddUser), newUser);
            }
            return BadRequest();   
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUser(int id)
        {
            if(await _userServices.DeleteUser(id))
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpPut]
        public async Task<ActionResult> EditUser(User user)
        {
            if(await _userServices.EditUser(user))
            {
                return Ok(await _userServices.GetUserById(user.Id));
            }

            return BadRequest();
        }
    }
}
