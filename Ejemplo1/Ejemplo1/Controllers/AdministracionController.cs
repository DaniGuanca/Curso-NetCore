using Ejemplo1.Models;
using Ejemplo1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ejemplo1.Controllers
{
    [Authorize(Roles = "Administrador,Dios")]
    public class AdministracionController : Controller
    {
        //La clase que se encarga de la administracion de roles
        private readonly RoleManager<IdentityRole> gestionRoles;

        //La clase que administra los usuarios, la necesito para sacar los usuarios por rol
        private readonly UserManager<UsuarioAplicacion> gestionUsuarios;

        //Constructor
        public AdministracionController(RoleManager<IdentityRole> gestionRoles, UserManager<UsuarioAplicacion> gestionUsuarios)
        {
            this.gestionRoles = gestionRoles;

            this.gestionUsuarios = gestionUsuarios;
        }


        [HttpGet]
        [Authorize]
        [Route("Administracion/CrearRol")]
        public IActionResult CrearRol()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [Route("Administracion/CrearRol")]
        public async Task<IActionResult> CrearRol(CrearRolViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Guardo el nombre que viene del form en un objeto de la clase identityRole
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.NombreRol
                };

                //Creo el nuevo rol con el objeto recien creado y los metodos del RoleManager
                IdentityResult result = await gestionRoles.CreateAsync(identityRole);

                //Si todo sale bien voy al index
                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }

                //Si llega a haber errores durante la creacion los recorro y los muestro en la vista
                //no es en un else porque dentro del if hay un return osea que si todo sale bien corta ahi.
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }


        //Muestro todos los roles
        [HttpGet]
        [Authorize]
        [Route("Administracion/Roles")]
        public IActionResult ListaRoles()
        {
            var roles = gestionRoles.Roles;
            return View(roles);
        }

        //Editar Roles
        [HttpGet]
        [Route("Administracion/EditarRol")]
        public async Task<IActionResult> EditarRol(string id)
        {
            //busca rol con los metodos del Role Manager
            var rol = await gestionRoles.FindByIdAsync(id);

            //veo si lo encontro o no
            if (rol == null)
            {
                ViewBag.ErrorMessage = $"Rol con el id: {id} no fue encontrado";

                return View("Error");
            }

            //Guardo los resultados de la busqueda en mi clase del modelo
            var model = new EditarRolViewModel
            {
                Id = rol.Id,
                RolNombre = rol.Name
            };

            //Obtengo todos los usuarios con ese rol
            foreach (var usuario in gestionUsuarios.Users)
            {
                if (await gestionUsuarios.IsInRoleAsync(usuario, rol.Name))
                {
                    model.Usuarios.Add(usuario.UserName);
                }
            }

            return View(model);
        }

        [HttpPost]
        [Route("Administracion/EditarRol")]
        public async Task<IActionResult> EditarRole(EditarRolViewModel model)
        {
            var rol = await gestionRoles.FindByIdAsync(model.Id);

            if (rol == null)
            {
                ViewBag.ErrorMessage = $"Rol con el id: {model.Id} no fue encontrado";

                return View("Error");
            }
            else
            {
                rol.Name = model.RolNombre;

                //Hago la modificaion por los metodos del role manager
                var resultado = await gestionRoles.UpdateAsync(rol);

                if (resultado.Succeeded)
                {
                    return RedirectToAction("ListaRoles");
                }

                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }

        }


        [HttpPost]
        [Route("Administracion/EliminarRol")]
        [Authorize(Policy = "BorrarRolPolicy")]
        public async Task<IActionResult> EliminarRol(string id)
        {
            var role = await gestionRoles.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Rol con id: {id} no encontrado";
                return View("Error");

            }
            else
            {
                var resultado = await gestionRoles.DeleteAsync(role);

                if (resultado.Succeeded)
                {
                    return RedirectToAction("ListaRoles");
                }

                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListaRoles");
            }
        }




        //Para la vista de eliminar o agregar un usuario a un rol
        [HttpGet]
        [Route("Administracion/EditarUsuarioRol")]
        public async Task<IActionResult> EditarUsuarioRol(string rolId)
        {
            ViewBag.roleId = rolId;

            //Obtengo el rol del id pasado
            var role = await gestionRoles.FindByIdAsync(rolId);

            //Me fijo si vino vacio
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Rol con el id {rolId} no existe";
                return View("Error");
            }

            //Creo una lista usuario rol modelo
            var model = new List<UsuarioRolModelo>();

            //Recorro todos los usuarios y los meto a la lista, ademas me fijo si pertenecen al rol y si es asi le checkeo el check
            foreach (var user in gestionUsuarios.Users)
            {
                var usuarioRolModelo = new UsuarioRolModelo
                {
                    UsuarioId = user.Id,
                    UsuarioNombre = user.UserName
                };

                if (await gestionUsuarios.IsInRoleAsync(user, role.Name))
                {
                    usuarioRolModelo.EstaSeleccionado = true;
                }
                else
                {
                    usuarioRolModelo.EstaSeleccionado = false;
                }

                model.Add(usuarioRolModelo);
            }

            return View(model);

        }

        [HttpPost]
        [Route("Administracion/EditarUsuarioRol")]
        //[Authorize(Policy = "EditarRolPolicy")]
        public async Task<IActionResult> EditarUsuarioRol(List<UsuarioRolModelo> model, string rolId)
        {
            var role = await gestionRoles.FindByIdAsync(rolId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Rol con el id {rolId} no existe";
                return View("Error");
            }

            for (int i = 0; i < model.Count; i++)
            {
                //Me traigo el usuario
                var user = await gestionUsuarios.FindByIdAsync(model[i].UsuarioId);

                //Para guardar el resultado de la operacion sobre la bd
                IdentityResult result = null;

                //Si esta seleccionado y no pertenece al rol quiere decir que presiono agregar entonces lo agrega
                if (model[i].EstaSeleccionado && !(await gestionUsuarios.IsInRoleAsync(user, role.Name)))
                {
                    result = await gestionUsuarios.AddToRoleAsync(user, role.Name);
                }
                else
                //Si no esta seleccionado y pertenece al rol entoces lo saco del rol
                if (!model[i].EstaSeleccionado && await gestionUsuarios.IsInRoleAsync(user, role.Name))
                {
                    result = await gestionUsuarios.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditarRol", new { id = rolId });
                }
            }

            return RedirectToAction("EditarRol", new { Id = rolId });
        }


        [HttpGet]
        [Route("Administracion/ListaUsuarios")]
        public IActionResult ListaUsuarios()
        {
            var usuarios = gestionUsuarios.Users;

            return View(usuarios);
        }


        //EDITAR USUARIOS
        [HttpGet]
        [Route("Administracion/EditarUsuario")]
        public async Task<IActionResult> EditarUsuario(string id)
        {
            var usuario = await gestionUsuarios.FindByIdAsync(id);

            if (usuario == null)
            {
                ViewBag.ErrorMessage = $"Usuario con id: {id} no encontrado";
                return View("Error");
            }

            //Traigo la lista de notificaciones del usuario
            var usuarioClaims = await gestionUsuarios.GetClaimsAsync(usuario);

            //Traigo sus roles
            var usuarioRoles = await gestionUsuarios.GetRolesAsync(usuario);

            //Lleno el modelo para mandarle a la vista
            var model = new EditarUsuarioModelo
            {
                Id = usuario.Id,
                NombreUsuario = usuario.UserName,
                Email = usuario.Email,
                AyudaPass = usuario.ayudaPass,
                //La paso a lista
                Notificaciones = usuarioClaims.Select(c => c.Value).ToList(),
                Roles = usuarioRoles
            };

            return View(model);
        }


        [HttpPost]
        [Route("Administracion/EditarUsuario")]
        public async Task<IActionResult> EditarUsuario(EditarUsuarioModelo model)
        {
            var usuario = await gestionUsuarios.FindByIdAsync(model.Id);

            if (usuario == null)
            {
                ViewBag.ErrorMessage = $"Usuario con id: {model.Id} no encontrado";
                return View("Error");
            }

            usuario.Id = model.Id;
            usuario.UserName = model.NombreUsuario;
            usuario.Email = model.Email;
            usuario.ayudaPass = model.AyudaPass;

            var resultado = await gestionUsuarios.UpdateAsync(usuario);

            if (resultado.Succeeded)
            {
                return RedirectToAction("ListaUsuarios");
            }

            foreach (var error in resultado.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }


        [HttpPost]
        [Route("Administracion/EliminarUsuario")]
        public async Task<IActionResult> BorrarUsuario(string id)
        {
            var user = await gestionUsuarios.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Usuario con id: {id} no encontrado";
                return View("Error");

            }
            else
            {
                var resultado = await gestionUsuarios.DeleteAsync(user);

                if (resultado.Succeeded)
                {
                    return RedirectToAction("ListaUsuarios");
                }

                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListaUsuarios");
            }
        }

        [HttpGet]
        [Route("Administracion/GestionarRolesUsuario")]
        [Authorize(Policy ="EditarRolPolicy")]
        public async Task<IActionResult> GestionarRolesUsuario(string IdUsuario)
        {

            ViewBag.IdUsuario = IdUsuario;

            var user = await gestionUsuarios.FindByIdAsync(IdUsuario);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Usuario con id: {IdUsuario} no encontrado";
                return View("Error");

            }

            var model = new List<RolUsuarioModelo>();

            foreach (var rol in gestionRoles.Roles)
            {
                var rolUsuarioModelo = new RolUsuarioModelo
                {
                    RolId = rol.Id,
                    RolNombre = rol.Name
                };

                if (await gestionUsuarios.IsInRoleAsync(user, rol.Name))
                {
                    rolUsuarioModelo.EstaSeleccionado = true;
                }
                else
                {
                    rolUsuarioModelo.EstaSeleccionado = false;
                }

                model.Add(rolUsuarioModelo);
            }

            return View(model);

        }


        [HttpPost]
        [Route("Administracion/GestionarRolesUsuario")]
        [Authorize(Policy = "EditarRolPolicy")]
        public async Task<IActionResult> GestionarRolesUsuario(List<RolUsuarioModelo> model, string IdUsuario)
        {
            var user = await gestionUsuarios.FindByIdAsync(IdUsuario);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Usuario con id: {IdUsuario} no encontrado";
                return View("Error");

            }


            //Primero les saco todos los roles para despues volver a agregar solo los que esten tildados
            var roles = await gestionUsuarios.GetRolesAsync(user);
            var result = await gestionUsuarios.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "No podemos borrar usuario con roles");
                return View(model);
            }

            //En el segundo parametro basicamente es una consulta que selecciona los nombre de los roles donde el campo estaSeleccionado
            //esta en true
            result = await gestionUsuarios.AddToRolesAsync(user,
                model.Where(x => x.EstaSeleccionado).Select(y => y.RolNombre));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "No podemos añadir los roles al usuario seleccionado");
                return View(model);
            }

            return RedirectToAction("EditarUsuario", new { Id = IdUsuario });
        }



        [HttpGet]
        [Route("Administracion/GestionarUsuariosClaims")]
        public async Task<IActionResult> GestionarUsuarioClaims(string IdUsuario)
        {
            var usuario = await gestionUsuarios.FindByIdAsync(IdUsuario);

            if (usuario == null)
            {
                ViewBag.ErrorMessage = $"Usuario con id: {IdUsuario} no encontrado";
                return View("Error");

            }

            //Obtengo todos los claims del usuario
            var existingUserClaims = await gestionUsuarios.GetClaimsAsync(usuario);

            var modelo = new UsuarioClaimsViewModel
            {
                idUsuario = IdUsuario
            };

            //Recorremos los claims de la app que tengo guardado en almacenClaims y los guardo en la lista del modelo
            foreach (Claim claim in AlmacenClaims.todosLosClaims)
            {
                UsuarioClaim usuarioClaim = new UsuarioClaim
                {
                    tipoClaim = claim.Type
                };

                //Si el usuario tiene el claim le pongo el bool esta seleccionado en true
                //Agrega la parte de true en el video 55
                if (existingUserClaims.Any(c => c.Type == claim.Type && c.Value == "true"))
                {
                    usuarioClaim.estaSeleccionado = true;
                }

                modelo.Claims.Add(usuarioClaim);
            }

            return View(modelo);
        }


        [HttpPost]
        [Route("Administracion/GestionarUsuariosClaims")]
        public async Task<IActionResult> GestionarUsuarioClaims(UsuarioClaimsViewModel modelo)
        {
            var usuario = await gestionUsuarios.FindByIdAsync(modelo.idUsuario);

            if (usuario == null)
            {
                ViewBag.ErrorMessage = $"Usuario con id: {modelo.idUsuario} no encontrado";
                return View("Error");
            }

            //Similar al metodo post de roles, obtengo los claims y los borro
            var existingUserClaims = await gestionUsuarios.GetClaimsAsync(usuario);
            var result = await gestionUsuarios.RemoveClaimsAsync(usuario, existingUserClaims);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "No podemos borrar usuario con roles");
                return View(modelo);
            }

            //Le asociamos los claims que tienen el estado seleccionado
            //Video 54
            //result = await gestionUsuarios.AddClaimsAsync(usuario,
            //    modelo.Claims.Where(c => c.estaSeleccionado).Select(c => new Claim(c.tipoClaim, c.tipoClaim)));

            //Video 55
            //Le pone un valor true si esta seleccionado
            result = await gestionUsuarios.AddClaimsAsync(usuario,
                modelo.Claims.Select(c => new Claim(c.tipoClaim, c.estaSeleccionado ? "true" : "false")));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "No podemos añadir los claims al usuario seleccionado");
                return View(modelo);
            }

            return RedirectToAction("EditarUsuario", new { Id = modelo.idUsuario });

        }
    }
}
