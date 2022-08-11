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

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string AyudaPass { get; set; }

        public List<string> Notificaciones { get; set; }

        public IList<string> Roles { get; set; }

    }
}
