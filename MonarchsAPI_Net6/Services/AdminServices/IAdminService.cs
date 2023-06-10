using MonarchsAPI_Net6.DTOs.AdminDtos;

namespace MonarchsAPI_Net6.Services.AdminServices
{
    public interface IAdminService
    {
        Task<bool> RegisterAdmin(AdminLoginRequest requestDto);
        Task<AdminLoginResponse> LoginAdmin(AdminLoginRequest requestDto);
    }
}
