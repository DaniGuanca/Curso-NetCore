using Ejemplo1.Models;
using Ejemplo1.Seguridad;
using Ejemplo1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo1.Controllers
{
    [Authorize]
    public class HomeController:Controller
    {
        private IAmigoAlmacen amigoAlmacen;
        private IHostingEnvironment hosting;
        private readonly IDataProtector protector;

        public HomeController(IAmigoAlmacen AmigoAlmacen ,IHostingEnvironment hostingEnvironment,
            IDataProtectionProvider dataProtectionProvider,
            ProteccionStrings proteccionStrings)
            
        {
            amigoAlmacen = AmigoAlmacen;
            hosting = hostingEnvironment;
            protector = dataProtectionProvider.CreateProtector(proteccionStrings.AmigoIdRuta);
        }

        [Route("")]
        [Route("Home")]
        [Route("Home/Index")]
       
        public ViewResult Index()
        {
            var modelo = amigoAlmacen.DameTodosLosAMigos().Select(e =>
            {
                // Ciframos el valor de ID y almacénarlo en la propiedad IdEncriptado
                e.IdEncriptado = protector.Protect(e.Id.ToString());
                return e;
            }); 
            return View(modelo);
        }
        [Route("Home/Details/{id}")]
       
      
        public ViewResult Details(string id)
        {
            int IdDesencriptado = Convert.ToInt32(protector.Unprotect(id));

            DetallesView detalles = new DetallesView();
            detalles.amigo= amigoAlmacen.dameDatosAmigo(IdDesencriptado);
            detalles.Titulo = "LISTA AMIGOS VIEW MODELS";

            if (detalles.amigo == null)
            {
                Response.StatusCode = 404;
                return View("AmigoNoEncontrado", IdDesencriptado);
            }
     


            return View(detalles);
        }

        [Route("Home/Create")]
        [HttpGet]
        
        public ViewResult Create()
        {
           return View();
        }
        
        [HttpPost]
        [Route("Home/Create")]
       
        public IActionResult Create(CrearAmigoModelo a)
        {
            if (ModelState.IsValid)
            {
                string guidImagen = null;
                if (a.Foto != null)
                {
                    string ficherosImagenes= Path.Combine(hosting.WebRootPath, "images");
                    guidImagen = Guid.NewGuid().ToString() + a.Foto.FileName;
                    string rutaDefinitiva = Path.Combine(ficherosImagenes, guidImagen);
                    a.Foto.CopyTo(new FileStream(rutaDefinitiva, FileMode.Create));
                }

                Amigo nuevoAmigo = new Amigo();
                nuevoAmigo.Nombre = a.Nombre;
                nuevoAmigo.Email = a.Email;
                nuevoAmigo.Ciudad = a.Ciudad;
                nuevoAmigo.rutaFoto = guidImagen;

                amigoAlmacen.nuevo(nuevoAmigo);
                return RedirectToAction("details", new { id = nuevoAmigo.Id });
            }

            return View();
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Amigo amigo = amigoAlmacen.dameDatosAmigo(id);
            EditarAmigoModelo amigoEditar = new EditarAmigoModelo
            {
                Id = amigo.Id,
                Nombre = amigo.Nombre,
                Email = amigo.Email,
                Ciudad = amigo.Ciudad,
                rutaFotoExistente = amigo.rutaFoto
            };
            return View(amigoEditar);
        }

        [HttpPost]
        public IActionResult Edit(EditarAmigoModelo model)
        {
            //Comprobamos que los datos son correctos
            if (ModelState.IsValid)
            {
                // Obtenemos los datos de nuestro amigo de la BBDD
                Amigo amigo = amigoAlmacen.dameDatosAmigo(model.Id);
                // Actualizamos los datos de nuestro objeto del modelo
                amigo.Nombre = model.Nombre;
                amigo.Email = model.Email;
                amigo.Ciudad = model.Ciudad;

                
                if (model.Foto != null)
                {
                    //Si el usuario sube una foto.Debe borrarse la anterior
                    if (model.rutaFotoExistente != null)
                    {
                        string ruta = Path.Combine(hosting.WebRootPath,"images", model.rutaFotoExistente);
                        System.IO.File.Delete(ruta);
                    }
                    //Guardamos la foto en wwwroot/images
                    amigo.rutaFoto = SubirImagen(model);
                }

                Amigo amigoModificado = amigoAlmacen.modificar(amigo);

                return RedirectToAction("index");
            }

            return View(model);
        }

        private string SubirImagen(EditarAmigoModelo model)
        {
            string nombreFichero = null;

            if (model.Foto != null)
            {
                string carpetaSubida = Path.Combine(hosting.WebRootPath, "images");
                nombreFichero = Guid.NewGuid().ToString() + "_" + model.Foto.FileName;
                string ruta = Path.Combine(carpetaSubida, nombreFichero);
                using (var fileStream = new FileStream(ruta, FileMode.Create))
                {
                    model.Foto.CopyTo(fileStream);
                }
            }

            return nombreFichero;
        }
    }
}
