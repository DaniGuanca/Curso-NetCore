using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo1.Models
{
    public class Amigo
    {
        public int Id { get; set; }

        [NotMapped]
        public string IdEncriptado { get; set; }

        [Required(ErrorMessage = "Obligatorio"), MaxLength(100, ErrorMessage = "No más de 100 carácteres")]
        public string Nombre { get; set; }
        
        [Required(ErrorMessage ="Obligatorio")]
        [Display(Name = "Email")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Formato incorrecto")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una ciudad")]
        public Provincia? Ciudad { get; set; }

        public string rutaFoto { get; set; }

    }
}
