using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo1.ViewModels
{
    public class OlvidoPasswordViewModel
    {
        [Required(ErrorMessage = "Email Obligatorio")]
        [EmailAddress(ErrorMessage = "Formato incorrecto")]
        public string Email { get; set; }
    }
}
