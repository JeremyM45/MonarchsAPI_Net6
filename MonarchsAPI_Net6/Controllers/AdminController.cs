using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonarchsAPI_Net6.DTOs.AdminDtos;
using MonarchsAPI_Net6.Models;
using MonarchsAPI_Net6.Services.AdminServices;

namespace MonarchsAPI_Net6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("all"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<AdminGetResponseDto>>> GetAllAdmins()
        {
            return Ok(await _adminService.GetAllAdmins());
        }

        [HttpGet("from-id"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<AdminGetResponseDto>> GetAdminById(int id)
        {
            AdminGetResponseDto? admin = await _adminService.GetAdminById(id);
            if(admin == null)
            {
                return BadRequest("ERROR: Could not find admin");
            }
            return Ok(admin);
        }

        [HttpDelete, Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteAdmin(int id)
        {
            if(await _adminService.DeleteAdmin(id))
            {
                return NoContent();
            }
            return BadRequest("ERROR: Could not delete admin");
        }

        [HttpPost("register"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> RegisterAdmin(AdminLoginRequest requestDto)
        {
            if (await _adminService.RegisterAdmin(requestDto))
            {
                return Created(nameof(RegisterAdmin), requestDto.Username);
            }
            return BadRequest("Could not add new admin");
        }
        [HttpPost("login")]
        public async Task<ActionResult<AdminLoginResponse>> LoginAdmin(AdminLoginRequest requestDto)
        {
            AdminLoginResponse? responseDto = await _adminService.LoginAdmin(requestDto);
            if (responseDto != null)
            {
                return Ok(responseDto);
            }
            return BadRequest("Could not login. Username or Password may be incorrect");
        }

    }
}
