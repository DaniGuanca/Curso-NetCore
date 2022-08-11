using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ejemplo1.Models
{
    public static class AlmacenClaims
    {
        public static List<Claim> todosLosClaims = new List<Claim>()
        {
            new Claim("Crear Rol", "Crear Rol"),
            new Claim("Editar Rol", "Editar Rol"),
            new Claim("Borrar Rol", "Borrar Rol")
        };
    }
}
