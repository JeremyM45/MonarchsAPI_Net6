using Microsoft.EntityFrameworkCore;
using MonarchsAPI_Net6.Data;
using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.Services.DynastyServices
{
    public class DyanstyServices : IDynastyServices
    {
        private readonly DataContext _dataContext;

        public DyanstyServices(DataContext context)
        {
            _dataContext = context;
        }
        public async Task<List<Dynasty>> GetAll()
        {
            return await _dataContext.Dynasties.ToListAsync();
        }
    }
}
