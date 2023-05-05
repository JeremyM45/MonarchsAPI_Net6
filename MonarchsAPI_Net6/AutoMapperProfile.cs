﻿using AutoMapper;
using MonarchsAPI_Net6.DTOs.DyanstyDtos;
using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Dynasty, DynastyResponseDto>();
        }
    }
}