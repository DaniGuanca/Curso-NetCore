using Ejemplo1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ejemplo1.Controllers
{
    //Solo pueden acceder usuarios logueados
    [Authorize]
    public class CuentasController : Controller
    {
        //La clase UserManager nos da la API que permite administrar y gestionar usuarios
        private readonly UserManager<UsuarioAplicacion> gestionUsuarios;

        //La clase SignInManager nos da la API que tiene los metodos necesarios para que el usuario inicie sesion
        private readonly SignInManager<UsuarioAplicacion> gestionLogin;

        private readonly ILogger<CuentasController> log;

        //Constructor
        public CuentasController(UserManager<UsuarioAplicacion> gestionUsuarios, SignInManager<UsuarioAplicacion> gestionLogin,
            ILogger<CuentasController> log)
        {
            this.gestionUsuarios = gestionUsuarios;
            this.gestionLogin = gestionLogin;
            this.log = log;
        }


        //La vista de Registro
        [HttpGet]
        [Route("Cuentas/Registro")]
        //Permito que entre cualquiera para poder registrarse
        [AllowAnonymous]
        public IActionResult Registro()
        {
            return View();
        }

        //POST es asincrona
        [HttpPost]
        [Route("Cuentas/Registro")]
        //Permito que entre cualquiera para poder registrarse
        [AllowAnonymous]
        public async Task<IActionResult> Registro(RegistroModelo model)
        {
            //Si pasa las validaciones
            if(ModelState.IsValid)
            {
                //Pasamos los valores de la clase RegistroModelo a una clase IdentityUser que es la usa el UserManager y SignInManager
                var usuario = new UsuarioAplicacion
                {
                    //El nombre del usuario va a ser el email como se hace actualmente en varias paginas
                    UserName = model.Email,
                    Email = model.Email,

                    //Atributo identity personalizado
                    ayudaPass = model.ayudaPass
                };

                //Guardo al usuario con contraseña la contraseña que paso usando el metodo createAsync del UserManager 
                //en la tabla aspUsers que creo el identity
                var resultado = await gestionUsuarios.CreateAsync(usuario, model.Password);

                //Valido si se creo con exito
                if (resultado.Succeeded)
                {
                    //TOKEN PARA CONFIRMACION DE MAIL
                    var token = await gestionUsuarios.GenerateEmailConfirmationTokenAsync(usuario);

                    var linkConfirmacion = "https://localhost:44338/Cuentas/ConfirmarEmail?UsuarioId=" + usuario.Id + "&token" + WebUtility.UrlDecode(token);

                    //Como no estoy enviando mails de confirmacion porque no tengo servidor ftp y en el curso
                    //el tipo tiene paja de hacerlo, mete el link de confirmacion en el log para que lo saque
                    //durante la depuracion manualmente y copie y pegue en el navegador
                    log.Log(LogLevel.Error, linkConfirmacion);

                    //Si el usuario es un administrador, una vez creado el usuario me manda de nuevo a la lista de gestion de usuarios
                    if(gestionLogin.IsSignedIn(User) && User.IsInRole("Administrador"))
                    {
                        return RedirectToAction("ListaUsuarios", "Administracion");
                    }


                    ViewBag.ErrorTitle = "Registro Correcto";
                    ViewBag.ErrorMessage = "Antes de iniciar sesion confirma el registro clickeando en el mail recibido";
                    return View("Error");
                    ////Intento loguear con SignInManager y un await asi espero que se loguee y recien entra a la linea que 
                    ////redirige al index
                    //await gestionLogin.SignInAsync(usuario, isPersistent: false);

                    ////RedirectToAction(metodo,controlador)
                    //return RedirectToAction("index", "home");
                }

                //En caso de que se produzca un error lo controlo
                foreach(var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }


        //METODO CONFIRMAR EMEAIL
        [AllowAnonymous]
        [Route("Cuentas/ConfirmarEmail")]
        public async Task<IActionResult> ConfirmarEmail(string usuarioId, string token)
        {
            if(usuarioId == null || token == null)
            {
                return RedirectToAction("index", "home");
            }

            var usuario = await gestionUsuarios.FindByIdAsync(usuarioId);
            if(usuario == null)
            {
                ViewBag.ErrorMessage = $"El usuario con id {usuarioId} es invalido";
                return View("ErrorGenerico");
            }

            var result = await gestionUsuarios.ConfirmEmailAsync(usuario, token);
            if (result.Succeeded)
            {
                return View();
            }

            ViewBag.ErrorTitle = "El email no pudo ser confirmado";
            return View("Error Generico");
        }

        [HttpPost]
        [Route("Cuentas/CerrarSesion")]
        public async Task<IActionResult> CerrarSesion()
        {
            await gestionLogin.SignOutAsync();

            return RedirectToAction("index", "home");
        }

        [HttpGet]
        [Route("Cuentas/Login")]
        //Permito que entre cualquiera para poder loguearse
        [AllowAnonymous]
        public async Task<IActionResult> Login(string urlRetorno)
        {
            LoginViewModelo modelo = new LoginViewModelo
            {
                UrlRetorno = urlRetorno,
                LoginExternos = (await gestionLogin.GetExternalAuthenticationSchemesAsync()).ToList()
            };

             return View(modelo);
        }


        //Login externo para autenticacion por google y facebook
        [HttpPost]
        [Route("Cuentas/LoginExterno")]
        //Permito que entre cualquiera para poder loguearse
        [AllowAnonymous]
        public IActionResult LoginExterno(string proveedor, string urlRetorno)
        {

            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = urlRetorno });

            var properties = gestionLogin.ConfigureExternalAuthenticationProperties(proveedor, redirectUrl);

            return new ChallengeResult(proveedor, properties);

        }


        //Una vez logueado externamente
        [Route("Cuentas/LoginExterno")]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null , string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            LoginViewModelo loginViewModelo = new LoginViewModelo
            {
                UrlRetorno = returnUrl,
                LoginExternos = (await gestionLogin.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState
                    .AddModelError(string.Empty, $"Error en el proveedor externo: {remoteError}");

                return View("Login", loginViewModelo);
            }

            //Obtenemos informacion del inicio de sesion del proveedor externo
            var info = await gestionLogin.GetExternalLoginInfoAsync();

            if(info == null)
            {
                ModelState.AddModelError(string.Empty, "Error cargando la informacion");

                return View("Login", loginViewModelo);
            }


            //Obtengo el claim por correo electronico del proveedor de inicio de sesion externo(Google, Facebook, etc)
            var email2 = info.Principal.FindFirstValue(ClaimTypes.Email);
            UsuarioAplicacion usuario = null;

            if(email2 != null)
            {
                //Buscamos el usuario
                usuario = await gestionUsuarios.FindByEmailAsync(email2);

                //Si el mail no esta confirmado muestra el error en la login view
                if (usuario != null && !usuario.EmailConfirmed)
                {
                    ModelState.AddModelError(string.Empty, "Email sin confirmar");
                    return View("Login", loginViewModelo);
                }
            }


            //Si el usuario ya tiene inicio de sesion (si hay un registro en AspNetUserLogins tabla) inicia sesion con el usuario
            //de este proveedor externo
            var loginResultado = await gestionLogin.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false,
                bypassTwoFactor: true);

            if (loginResultado.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            //Si no hay registro en la tabla AspNetUserLogins el usuario puede no tener una cuenta local
            else
            {
                //obtenemos el claim
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if(email != null)
                {
                    //creo nuevo usuario sin contraseña si todavia no tenemos usuario
                    var user = await gestionUsuarios.FindByEmailAsync(email);

                    if(user == null)
                    {
                        user = new UsuarioAplicacion
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };

                        await gestionUsuarios.CreateAsync(user);
                    }

                    //Agrego un inicio de sesion(una nueva fila para el usuario en la tabla aspNetUserLogins)
                    await gestionUsuarios.AddLoginAsync(user, info);
                    await gestionLogin.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }

                //Si no encontramos el correo electronico del usuario no podemos continuar
                ViewBag.ErrorTitle = $"Email claim no fue recibido de: {info.LoginProvider}";
                ViewBag.ErrorMessage = "Contacta con dany12rp13@gmail.com";

                return View("Error");
            }    
        }




        [HttpPost]
        [Route("Cuentas/Login")]
        //Permito que entre cualquiera para poder loguearse
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModelo model)
        {

            model.LoginExternos = (await gestionLogin.GetExternalAuthenticationSchemesAsync()).ToList();


            if(ModelState.IsValid)
            {
                var usuario = await gestionUsuarios.FindByEmailAsync(model.Email);

                if(usuario != null && !usuario.EmailConfirmed &&
                    (await gestionUsuarios.CheckPasswordAsync(usuario, model.Password)))
                {
                    ModelState.AddModelError(string.Empty, "Email todavia no confirmado");
                    return View(model);
                }

                var result = await gestionLogin.PasswordSignInAsync(
                    model.Email, model.Password, model.Recuerdame, false);
                
                if(result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }

                ModelState.AddModelError(string.Empty, "Inicio de sesion no valido");               

            }

            return View(model);
        }


        //Acepta tanto peticiones GET como POST
        [AcceptVerbs("Get","Post")]
        [AllowAnonymous]
        [Route("Cuentas/CombrobarEmail")]
        public async Task<IActionResult> ComprobarEmail(string email)
        {
            //El FindByEmailAsync es un metodo propio del UserManager
            var user = await gestionUsuarios.FindByEmailAsync(email);

            //Me fijo si existe un usuario con ese mail
            if ( user == null)
            {
                return Json(true);
            }
            else
            {
                //El signo $ delante de un string crea un string interpolado que basicamente permite meter variables mediante llaves {}
                //en vez de estar concatenando con "+ var +".
                return Json($"El email {email} no esta disponible");
            }

        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Cuentas/AccesoDenegado")]
        public IActionResult AccesoDenegado()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Cuentas/OlvidoPassword")]
        public IActionResult OlvidoPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Cuentas/OlvidoPassword")]
        public async Task<IActionResult> OlvidoPassword(OlvidoPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Buscamos el usuario por el email
                var usuario = await gestionUsuarios.FindByEmailAsync(model.Email);

                //Si el usuario existe y el email esta confirmado
                if(usuario != null && await gestionUsuarios.IsEmailConfirmedAsync(usuario))
                {
                    //Genero token de restablecimiento de contraseña
                    var token = await gestionUsuarios.GeneratePasswordResetTokenAsync(usuario);

                    //Hago el link para el reset
                    var linkReseteaPass = "https://localhost:44338/Cuentas/ReseteaPassword?Email=" + model.Email + "&token=" + WebUtility.UrlEncode(token);

                    //Lo guardo en el log para ver durante depuracion y copio y pego el link en el nav porque no
                    //tengo smtp para estar enviando el link por email
                    log.Log(LogLevel.Warning, linkReseteaPass);

                    //Enviar al usuario a la vista de confirmacion de contraseña olvidada
                    return View("OlvidoPasswordConfirmacion");
                }

                return View("OlvidoPasswordConfirmacion");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Cuentas/ReseteaPassword")]
        public IActionResult ReseteaPassword(string token, string email)
        {
            if(token == null || email == null)
            {
                ModelState.AddModelError("", "Link desconocido");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Cuentas/ReseteaPassword")]
        public async Task<IActionResult> ReseteaPassword(ResetPassViewModel model)
        {
            if(ModelState.IsValid)
            {
                var usuario = await gestionUsuarios.FindByEmailAsync(model.Email);

                if(usuario != null)
                {
                    var resultado = await gestionUsuarios.ResetPasswordAsync(usuario, model.Token, model.Password);
                    if(resultado.Succeeded)
                    {
                        return View("ResetearPasswordConfirmacion");
                    }

                    foreach(var error in resultado.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }

                return View("ResetearPasswordConfirmacion");
            }

            return View(model);
        }

        [HttpGet]
        [Route("Cuentas/CambiarPassword")]
        public IActionResult CambiarPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("Cuentas/CambiarPassword")]
        public async Task<IActionResult> CambiarPassword(CambioPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = await gestionUsuarios.GetUserAsync(User);
                if(usuario == null)
                {
                    return RedirectToAction("Login");
                }

                //Cambia la contraseña usando el metodo ChangePasswordAsync
                var result = await gestionUsuarios.ChangePasswordAsync(usuario,
                    model.PasswordActual, model.NuevaPassword);

                //Si la mueva contraseña no cumple con los requisitos o la contraseña actual es incorrecta
                if (!result.Succeeded)
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }

                //Al cambiar con exito la contraseña actualiza la cookie de inicio de sesion
                await gestionLogin.RefreshSignInAsync(usuario);
                return View("CambiarPasswordConfirmacion");
            }

            return View(model);
        }
    }
}
