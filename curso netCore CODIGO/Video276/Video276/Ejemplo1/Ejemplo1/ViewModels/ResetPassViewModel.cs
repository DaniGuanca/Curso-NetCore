using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo1.ViewModels
{
    public class ResetPassViewModel
    {
        [Required (ErrorMessage = "Email obligatorio") ]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Obligatorio")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar password")]
        [Compare("Password", ErrorMessage = "Password y su campo de confiramcion deben ser iguales")]
        public string ConfirmarPassword { get; set; }

        public string Token { get; set; }
    }
}
