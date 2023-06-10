using MonarchsAPI_Net6.DTOs.AdminDtos;
using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.Services.AdminServices
{
    public interface IAdminService
    {
        Task<bool> RegisterAdmin(AdminLoginRequest requestDto);
        Task<AdminLoginResponse> LoginAdmin(AdminLoginRequest requestDto);
        Task<bool> DeleteAdmin(int id);
        Task<List<Admin>> GetAllAdmins();
        Task<Admin> GetAdminById(int id);
    }
}
