﻿using System.ComponentModel.DataAnnotations;

namespace WebApiUser.Models.Dtos
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "El correo es obligatorio")]
        public string Email { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; }
        [Required(ErrorMessage = "El telefono es obligatorio")]
        public string Phone { get; set; }
    }
}
