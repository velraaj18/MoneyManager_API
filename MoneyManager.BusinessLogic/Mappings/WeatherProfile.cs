using System;
using AutoMapper;
using MoneyManager.DTO;
using MoneyManager.Models;

namespace MoneyManager.Mappings
{
    public class WeatherProfile : Profile
    {
        public WeatherProfile()
        {
            CreateMap<WeatherRequest, Weather>();
            CreateMap<Weather, WeatherRequest>();
        }
    }
}
