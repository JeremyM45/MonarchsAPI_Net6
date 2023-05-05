﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonarchsAPI_Net6.DTOs.CountryDtos;
using MonarchsAPI_Net6.Models;
using MonarchsAPI_Net6.Services.CountryServices;

namespace MonarchsAPI_Net6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryServices _countryServices;
        private readonly IMapper _mapper;
        public CountryController(ICountryServices countryServices, IMapper mapper)
        {
            _mapper = mapper;
            _countryServices = countryServices;
        }

        [HttpGet]
        public async Task<ActionResult<List<CountryResponseDto>>> GetAllCountries()
        {
            List<Country> countries = await _countryServices.GetAll();
            if(countries == null) { return BadRequest(); }
            return Ok(countries.Select(c => _mapper.Map<CountryResponseDto>(c)));
        }
    }
}
