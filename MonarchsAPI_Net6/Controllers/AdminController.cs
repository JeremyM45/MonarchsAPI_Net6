using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonarchsAPI_Net6.DTOs.AdminDtos;
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

        [HttpPost("register")]
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
