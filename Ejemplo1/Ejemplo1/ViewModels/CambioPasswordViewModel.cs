using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo1.ViewModels
{
    public class CambioPasswordViewModel
    {
        [Required(ErrorMessage = "Obligatorio")]
        [DataType(DataType.Password)]
        [Display(Name = "Password Actual")]
        public string PasswordActual { get; set;}

        [Required(ErrorMessage = "Obligatorio")]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva Password")]
        public string NuevaPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name ="Confirmar Nueva Password")]
        [Compare("NuevaPassword", ErrorMessage ="La password no coincide")]
        public string ConfirmarPassword { get; set; }
    }
}
