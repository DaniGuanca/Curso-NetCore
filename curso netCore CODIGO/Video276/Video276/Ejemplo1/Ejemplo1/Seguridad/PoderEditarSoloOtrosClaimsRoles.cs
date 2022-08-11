using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ejemplo1.Seguridad
{
    public class PoderEditarSoloOtrosClaimsRoles :AuthorizationHandler<GestionarAdminRolesyClaims>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            GestionarAdminRolesyClaims peticion)
        {
            var authFilterContext = context.Resource as AuthorizationFilterContext;
            if (authFilterContext == null)
            {
                return Task.CompletedTask;
            }

            string IdUsuarioLogado =
                context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            string adminIdEdicion = authFilterContext.HttpContext.Request.Query["IdUsuario"];

            if (context.User.IsInRole("Administrador") &&
                context.User.HasClaim(claim => claim.Type == "Editar Role" && claim.Value == "true") &&
                adminIdEdicion.ToLower() != IdUsuarioLogado.ToLower())
            {
                context.Succeed(peticion);
            }

            return Task.CompletedTask;
        }
    }
}
