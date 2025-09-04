using AutoMapper;
using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Application.Features.Users
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<User, UsersDto>().ReverseMap();
        }
    }
}
