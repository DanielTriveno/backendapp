
using AutoMapper;
using WebApiUser.Models;
using WebApiUser.Models.Dtos;

namespace WebApiUser.UsersMapper
{
    public class UsersMapper : Profile
    {
        public UsersMapper() {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
