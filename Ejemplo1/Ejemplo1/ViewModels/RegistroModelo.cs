using Ejemplo1.Utilidades;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo1.ViewModels
{
    public class RegistroModelo
    {
        [Required(ErrorMessage = "Email Obligatorio")]
        [Display(Name ="Email")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Formato Incorrecto")]
        //Valida un email
        [EmailAddress]
        //Validacion Remota
        [Remote (action: "ComprobarEmail", controller: "Cuentas")]
        //Validador Personalizado
        [ValidarNombreUsuario(usuario:"puto", ErrorMessage = "Palabra no permitida") ]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Obligatorio")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage ="Es requerida la confirmacion de contraseña")]
        [DataType(DataType.Password)]
        [Display(Name = "Repetir Password")]
        //Con compare compara esta propiedad con otra propiedad cuyo nombre le paso como string, si no coinciden tira el error
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string PasswordValidar { get; set; }

        [Display(Name = "Ayuda Password")]
        public string ayudaPass { get; set; }
    }
}
