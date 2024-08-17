using System.ComponentModel.DataAnnotations;

namespace WebApiUser.Models.Dtos
{
    public class UserLoginDto
    {
        
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string UserName { get; set; }
      
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; }
    
    }
}
