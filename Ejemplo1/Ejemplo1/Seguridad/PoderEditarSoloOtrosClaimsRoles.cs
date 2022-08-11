using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ejemplo1.Seguridad
{
    public class PoderEditarSoloOtrosClaimsRoles : AuthorizationHandler<GestionarAdminRolesyClaims>
    {
        //Metodo abstracto que hay que implementar
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GestionarAdminRolesyClaims peticion)
        {
            //Con esta herencia y metodo contro lo los requerimientos de autorizacion

            var authFilterContext = context.Resource as AuthorizationFilterContext;
            
            if(authFilterContext == null)
            {
                return Task.CompletedTask;
            }

            string IdUsuarioLogueado =
                context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            string adminIdEdicion = authFilterContext.HttpContext.Request.Query["IdUsuario"];


            //Si es administrador y tiene el claim de editar rol y ademas no esta editando sus propios roles
            if(context.User.IsInRole("Administrador") &&
                context.User.HasClaim(claim => claim.Type == "Editar Rol" && claim.Value == "true") &&
                adminIdEdicion.ToLower() != IdUsuarioLogueado.ToLower())
            {
                context.Succeed(peticion);
            }

            return Task.CompletedTask;

        }
    }
}
