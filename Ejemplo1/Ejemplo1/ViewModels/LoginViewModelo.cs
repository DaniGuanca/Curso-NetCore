using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo1.ViewModels
{
    public class LoginViewModelo
    {
        [Required(ErrorMessage = "Email obligatorio")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Obligatorio")]
        [MinLength(4, ErrorMessage ="4 Caracteres como minimo")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Recuerdame")]
        public bool Recuerdame { get; set; }

        public string UrlRetorno { get; set; }

        public IList<AuthenticationScheme> LoginExternos { get; set; }

    }
       
}
