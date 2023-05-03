﻿using Microsoft.EntityFrameworkCore;
using MonarchsAPI_Net6.Data;
using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.Services.CountryServices
{
    public class CountryServices : ICountryServices
    {
        private readonly DataContext _dataContext;
        public CountryServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Country>> GetAll()
        {
            List<Country> countries = await _dataContext.Countries.ToListAsync();
            return countries;
        }
    }
}
