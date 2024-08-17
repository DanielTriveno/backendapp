using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApiUser.Models;
using WebApiUser.Models.Dtos;
using WebApiUser.Repository.IRepository;

namespace WebApiUser.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _usRepo;
        protected ResponseAPI _responseApi;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository usRepo, IMapper mapper)
        {
            _usRepo = usRepo;
            _mapper = mapper;
            this._responseApi = new();
        }


        [HttpGet("{userId:int}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUser(int userId)
        {
            var user = _usRepo.GetUser(userId);
            if (user == null)
            {
                return NotFound();
            }
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

       
        [HttpPatch("{userId:int}", Name = "UpdatePatchUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public IActionResult UpdatePatchUser(int userId, [FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (userDto == null || userId != userDto.Id)
            {
                return BadRequest(ModelState);
            }
            var user = _mapper.Map<User>(userDto);

            if (!_usRepo.UpdateUser(user))
            {
                ModelState.AddModelError("", $"Algo salio actualizando el registro{user.UserName}");
                return StatusCode(404, ModelState);
            }
            return NoContent();
        }

        // DELETE api/users/{id}
        [HttpDelete("{userId:int}", Name ="DeleteUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteUser(int userId)
        {
   
            if (!_usRepo.ExistUser(userId))
            {
                return NotFound();
            }

            var user = _usRepo.GetUser(userId);

            if (!_usRepo.DeleteUser(user))
            {
                ModelState.AddModelError("", $"Algo salio mal al borrar usuario");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            bool validateUniqueUserName = _usRepo.IsUniqueUser(userRegisterDto.UserName);
            if (!validateUniqueUserName)
            {
                _responseApi.StatusCode = HttpStatusCode.BadRequest;
                _responseApi.IsSuccess = false;
                _responseApi.ErrorMessages.Add("El nombre de usuario ya existe");
                return BadRequest(_responseApi);
            }
            var user = await _usRepo.Register(userRegisterDto);
            if (user == null)
            {
                _responseApi.StatusCode = HttpStatusCode.BadRequest;
                _responseApi.IsSuccess = false;
                _responseApi.ErrorMessages.Add("Error en el registro");
                return BadRequest(_responseApi);
            }

            _responseApi.StatusCode = HttpStatusCode.OK;
            _responseApi.IsSuccess = true;
            return Ok(_responseApi);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var responseLogin = await _usRepo.Login(userLoginDto);

            if (responseLogin.User == null || string.IsNullOrEmpty(responseLogin.Token))
            {
                _responseApi.StatusCode = HttpStatusCode.BadRequest;
                _responseApi.IsSuccess = false;
                _responseApi.ErrorMessages.Add("El nombre de usuario o contraseña son incorrectos");
                return BadRequest(_responseApi);
            }
           
            _responseApi.StatusCode = HttpStatusCode.OK;
            _responseApi.IsSuccess = true;
            _responseApi.Result = responseLogin;
            return Ok(_responseApi);
        }
    }
}
