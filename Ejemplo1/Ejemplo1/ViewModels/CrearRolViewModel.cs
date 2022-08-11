using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo1.ViewModels
{
    public class CrearRolViewModel
    {
        //Validaciones
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Rol")]
        public string NombreRol { get; set; }
    }
}
