﻿using WebApiUser.Models;
using WebApiUser.Models.Dtos;

namespace WebApiUser.Repository.IRepository
{
    public interface IUserRepository
    {
        User GetUser(int userId);
        bool IsUniqueUser(string username);
        bool ExistUser(int userId);
        bool UpdateUser (User user);
        bool DeleteUser (User user);
        bool Save();
        Task<UserLoginResponseDto> Login ( UserLoginDto userLoginDto);
        Task<User> Register(UserRegisterDto userRegisterDto);

    }
}
