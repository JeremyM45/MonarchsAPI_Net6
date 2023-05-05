﻿using MonarchsAPI_Net6.DTOs.CountryDtos;
using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.Services.CountryServices
{
    public interface ICountryServices
    {
        Task<List<CountryResponseDto>> GetAll();
    }
}
