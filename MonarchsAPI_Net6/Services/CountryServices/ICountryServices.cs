using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.Services.CountryServices
{
    public interface ICountryServices
    {
        Task<List<Country>> GetAll();
    }
}
