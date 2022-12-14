using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo1.ViewModels
{
    public class EditarUsuarioModelo
    {
        public EditarUsuarioModelo()
        {
            Notificaciones = new List<string>();
            Roles = new List<string>();
        }

        public string Id { get; set; }

        [Required]
        public string NombreUsuario { get; set; }

        [Required (ErrorMessage ="* Obligatorio")]
        [EmailAddress(ErrorMessage = "Formato incorrecto")]
        public string Email { get; set; }

        public string ayudaPass { get; set; }

        public List<string> Notificaciones { get; set; }

        public IList<string> Roles { get; set; }
    }
}
