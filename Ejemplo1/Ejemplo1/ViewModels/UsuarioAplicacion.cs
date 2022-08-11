using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo1.ViewModels
{
    public class UsuarioAplicacion: IdentityUser
    {
        //creo la propiedad a añadir
        public string ayudaPass { get; set; }
    }
}
