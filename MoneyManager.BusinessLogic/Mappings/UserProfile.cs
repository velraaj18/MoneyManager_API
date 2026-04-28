using System;
using AutoMapper;
using MoneyManager.DTO;
using MoneyManager.Models;

namespace MoneyManager.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRequest, User>();
            CreateMap<User, UserRequest>();
        }
    }
}
