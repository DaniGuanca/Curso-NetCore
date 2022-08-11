using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo1.ViewModels
{
    public class ResetPassViewModel
    {
        [Required(ErrorMessage = "Email Obligatorio")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage ="Obligatorio")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Password")]
        [Compare("Password", ErrorMessage ="Password y confirmacion deben ser iguales")]
        public string ConfirmarPassword { get; set; }

        public string Token { get; set; }
    }
}
