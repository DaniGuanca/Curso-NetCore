using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo1.Seguridad
{
    public class SoySuperAdmin : AuthorizationHandler<GestionarAdminRolesyClaims>
    {
        protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        GestionarAdminRolesyClaims peticion)
        {
            if (context.User.IsInRole("Dios"))
            {
                context.Succeed(peticion);
            }

            return Task.CompletedTask;
        }
    }
}
