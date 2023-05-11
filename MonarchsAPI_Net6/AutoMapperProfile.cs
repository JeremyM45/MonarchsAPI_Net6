using AutoMapper;
using MonarchsAPI_Net6.DTOs.CountryDtos;
using MonarchsAPI_Net6.DTOs.DyanstyDtos;
using MonarchsAPI_Net6.DTOs.MonarchsDtos;
using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Dynasty, DynastyResponseDto>();
            CreateMap<Dynasty, DynastyResponseMinDto>();
            CreateMap<EditDynastyRequestDto, Dynasty>();
            CreateMap<CreateDynastyDto, Dynasty>();
            CreateMap<Country, CountryResponseDto>();
            CreateMap<Country, CountryResponseMinDto>();
            CreateMap<CreateCountryRequestDto, Country>();
            CreateMap<CreateMonarchRequestDto, Monarch>();
            CreateMap<Monarch, MonarchResponseDashboardDto>();
            CreateMap<Monarch, MonarchMinDto>();
            

        }
    }
}
