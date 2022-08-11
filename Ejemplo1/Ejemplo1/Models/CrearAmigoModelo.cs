using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo1.Models
{
    public class CrearAmigoModelo
    {
        //Esta clase es para manejar mejor la creacion de amigos por el tema de la imagen
        public int Id { get; set; }

        [Required(ErrorMessage = "Obligatorio"), MaxLength(100, ErrorMessage = "No mas de 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Obligatorio")]
        [Display(Name = "Email")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Formato Incorrecto")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una ciudad")]
        public Provincia? Ciudad { get; set; }

        public IFormFile Foto { get; set; }
    }
}
