using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiUser.Context;
using WebApiUser.Models;
using WebApiUser.Models.Dtos;
using WebApiUser.Repository.IRepository;
using XSystem.Security.Cryptography;

namespace WebApiUser.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;
        private string secretKey;

        public UserRepository(AppDbContext db, IConfiguration config) { 
           _db = db;
            secretKey = config.GetValue<string>("ApiSettings:Secret");
        }


        public bool ExistUser(int userId)
        {
            return _db.Users.Any( c => c.Id == userId );
        }

        public bool DeleteUser(User user)
        { 
           _db.Users.Remove(user);
            return Save();
        }

        public User GetUser(int userId)
        {
            return _db.Users.FirstOrDefault(c => c.Id == userId);
        }


        public bool IsUniqueUser(string username)
        {
            var userDb = _db.Users.FirstOrDefault(u => u.UserName == username);
            if (userDb == null)
            {
                return true;
            }
            return false;
        }
        public bool Save()
        {
            return _db.SaveChanges() > 0 ? true: false ;
        }

        public bool UpdateUser(User user)
        {
            var existingUser = _db.Users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser == null)
            {
                return false; // Si el usuario no existe, no se puede actualizar
            }

            if (!string.IsNullOrEmpty(user.Name)) existingUser.Name = user.Name;
            if (!string.IsNullOrEmpty(user.UserName) && IsUniqueUser(user.UserName)) existingUser.UserName = user.UserName;
            if (!string.IsNullOrEmpty(user.Email)) existingUser.Email = user.Email;
            if (!string.IsNullOrEmpty(user.Phone)) existingUser.Phone = user.Phone;

            if (!string.IsNullOrEmpty(user.Password))
            {
                existingUser.Password = user.Password;
            }

            _db.Users.Update(existingUser);
            return Save();
        }

        public async Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto)
        {
            var enPassword = getMd5(userLoginDto.Password);

            var user = _db.Users.FirstOrDefault(
                u => u.UserName.ToLower() == userLoginDto.UserName.ToLower()
                && u.Password == enPassword
                );
            if (user == null)
            {
                return new UserLoginResponseDto()
                {
                    Token = "",
                    User = null
                };
            }
            var handleToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials= new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = handleToken.CreateToken(tokenDescriptor);

            UserLoginResponseDto userLoginResponseDto = new UserLoginResponseDto()
            {
                Token = handleToken.WriteToken(token),
                User = user
            };

            return userLoginResponseDto;
        }

        public async Task<User> Register(UserRegisterDto userRegisterDto)
        {
            var enPassword = getMd5(userRegisterDto.Password);
            
            User user = new User()
            {
                UserName = userRegisterDto.UserName,
                Password = enPassword,
                Name = userRegisterDto.Name,
                Email = userRegisterDto.Email,
                Phone = userRegisterDto.Phone,
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            user.Password = enPassword;
            return user;

        }
        public static string getMd5(string value)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(value);
            data = x.ComputeHash(data);
            string resp = "";
            for (int i = 0; i < data.Length; i++)
                resp += data[i].ToString("x2").ToLower();
            return resp;
        }

    }
}
