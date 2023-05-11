using MonarchsAPI_Net6.DTOs.CountryDtos;
using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.Services.CountryServices
{
    public interface ICountryServices
    {
        Task<List<Country>> GetAll();
        Task<List<Country>> GetAllMin();
        Task<List<int>> GetCountryIdsByMonarch(int monarchId);
        Task<bool> AddCountry(CreateCountryRequestDto countryDto);
        Task<Country> EditCountry(EditCountryDto countryDto);
        Task<bool> DeleteCountry(int id);
    }
}
