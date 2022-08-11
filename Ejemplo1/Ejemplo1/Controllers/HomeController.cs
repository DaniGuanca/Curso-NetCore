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
    //Solo pueden acceder usuarios logueados
    [Authorize]
    public class HomeController : Controller
    {
        //La interfaz
        private IAmigoAlmacencs amigoAlmacen;

        //Para ver las rutas del servidor
        private IWebHostEnvironment hosting;

        //Codificar ID para URL
        private readonly IDataProtector protector;

        //El hosting para sacar rutas del servidor
        public HomeController(IAmigoAlmacencs AmigoAlmacen, IWebHostEnvironment hostingEnvironment,
            IDataProtectionProvider dataProtectionProvider, ProteccionStrings protectionsStrings)
        {
            amigoAlmacen = AmigoAlmacen;
            hosting = hostingEnvironment;
            //Codificar id
            protector = dataProtectionProvider.CreateProtector(protectionsStrings.AmigoIdRuta);
        }

        //public string Index()
        //{
        //    return amigoAlmacen.dameDatosAmigo(1).Email;
        //}

        //public JsonResult Details()
        //{
        //Voy a devolver en formato json el objeto amigo con id 1;
        //    Amigo modelo = amigoAlmacen.dameDatosAmigo(1);
        //     return Json(modelo);
        //}

        //LLAMADA A VISTAS DESDE EL CONTROLAR
        //Para llamar a las vistas desde el controlador con metodos usando el tipo ViewResult y retornando  un objeto View
        //Ej:
        //Ruta personalizada
        [Route ("")]
        [Route ("Home")]
        [Route ("Home/Index")]
        public ViewResult Index()
        {
            //var automaticamente le pone el tipo necesario
            //var modelo = amigoAlmacen.DameTodosLosAmigos();
            //Codificar id para url
            var modelo = amigoAlmacen.DameTodosLosAmigos().Select(e =>
            {
                //Ciframos el valor del ID y lo guardo en idEncriptado
                e.IdEncriptado = protector.Protect(e.Id.ToString());
                return e;
            });
            return View(modelo);
        }

        //Si quiero usar otras vistas que no son de la carpeta predetrminada puedo hacerlo mandando la url relativa con ~
        //public ViewResult Index()
        //{
        //    Amigo modelo = amigoAlmacen.dameDatosAmigo(2);
        //Aca en View le digo la direccion del html que quiero mostrar
        //    return View("~/MisVistas/index.cshtml");
        //}


        //VIDEO 16: PASANDO DATOS DE UN CONTROLADOR A LA VISTA
        //Esto son las 3 formas de hacerlo
        //public ViewResult Details()
        //{
        //    Amigo amigo = amigoAlmacen.dameDatosAmigo(1);

        //    //1° METODO ViewData
        //    //Para mandar datos del controlador a la vista lo hago con ViewData
        //    //se manda asi: ViewData["Nombre del dato"] = valor;
        //    //Entonces desde la vista lo agarro con Nombre del dato
        //    //Ej. Mando un titulo
        //    ViewData["Cabecera"] = "Lista de Amigos";

        //    //Y mando un objeto
        //    ViewData["Amigo"] = amigo;

        //    //2° METODO ViewBag
        //    //Es parecido a ViewData pero acá es ViewBag.NombreDato = valor;
        //    ViewBag.Titulo = "Lista de Amigos ViewBag";

        //    ViewBag.Amigo = amigo;

        //    return View(amigo);
        //}
        //Comento porque el tema del siguiente video tambien llama a Details y va a provocar error
        //*******************************************************************************************************************

        //VIDEO 17: VISTAS CON OBJETOS PERSONALIZADOS. ViewModels
        //Le paso parametro al viewResult
        // public ViewResult Details(int id)
        //Ruta personalizada por atributo
        [Route ("Home/Details/{id}")]
        [AllowAnonymous]
        public ViewResult Details(string id)
        //El signo de interrogacion despues del tipo de dato es para especificar que puede venir nulo o no
        {
            //Cifrar id para url
            string IdDesencriptadoCadena = protector.Unprotect(id);
            int IdDesencriptado = Convert.ToInt32(IdDesencriptadoCadena);

            DetailsView detalles = new DetailsView();
            //Meto los datos del amigo
            //detalles.amigo = amigoAlmacen.dameDatosAmigo(id?? 1);

            detalles.amigo = amigoAlmacen.dameDatosAmigo(IdDesencriptado);
            //Acá poner dos signos de interrogacion lo que hace es decir que ponga el id si viene y si no viene o es nulo busca con 1

            //El titulo y subtitulo que queria
            detalles.Titulo = "Lista Amigo ViewModels";
            detalles.Subtitulo = "xxxxxxxxxxxxxxxxxx";

            //Hago el control de que exisa el id
            if(detalles.amigo == null)
            {
                Response.StatusCode = 404;

                //Estoy llamando a la vista "AmigoNoEncontrado"
                return View("AmigoNoEncontrado", IdDesencriptado);
            }

            return View(detalles);
        }

        [Route ("Home/Create")]
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }


        //Acá manejo el metodo Post de Create
        //Uso el crearAmigoModelo por el tema de la foto, despues ya para la insercion en la BD uso Amigo
        [Route("Home/Create")]
        [HttpPost]

        public IActionResult Create(CrearAmigoModelo a)
        {
            //Este if es para comprobar que paso las validaciones que hice en la clase Amigo.cs
            if (ModelState.IsValid)
            {
                string guidImagen = null;

                if(a.Foto != null)
                {
                    //En la primer linea hago la ruta a la carpeta imagenes
                    //En la segunda linea creo un nombre unico para la imagen nueva, creo un identificador nuevo y lo combino con el
                    //nombre de la imagen para que no hayan dos con el mismo nombre y explote
                    string ficherosImagenes = Path.Combine(hosting.WebRootPath, "images");
                    guidImagen = Guid.NewGuid().ToString() + a.Foto.FileName;

                    //En la primer linea hago la ruta del archivo
                    //Y en la segunda line ya la subo la imagen al destino ruta
                    string rutaDefinitiva = Path.Combine(ficherosImagenes, guidImagen);
                    a.Foto.CopyTo(new FileStream(rutaDefinitiva, FileMode.Create));
                }


                Amigo nuevoAmigo = new Amigo();
                nuevoAmigo.Nombre = a.Nombre;
                nuevoAmigo.Email = a.Email;
                nuevoAmigo.Ciudad = a.Ciudad;
                //En la base de datos quiero guardar solo el identificador unico no la ruta completa
                nuevoAmigo.rutaFoto = guidImagen;

                amigoAlmacen.nuevo(nuevoAmigo);
                return RedirectToAction("details", new { id = nuevoAmigo.Id });
            }

            //vuelvo a la misma vista si no es valido para mostrar los errores de validacion
            return View();
            
        }


        [Route("Home/Edit/id")]
        [HttpGet]
        public ViewResult Edit(int id)
        {
            Amigo amigo = amigoAlmacen.dameDatosAmigo(id);

            //Guardo los datos en el viewmodel que hereda de crearAmigoModelo, es para manejar mejor la foto
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

        [Route("Home/Edit/id")]
        [HttpPost]
        public IActionResult Edit(EditarAmigoModelo a)
        {
            //Compruebo validaciones
            if(ModelState.IsValid)
            {
                Amigo amigo = amigoAlmacen.dameDatosAmigo(a.Id);

                amigo.Nombre = a.Nombre;
                amigo.Ciudad = a.Ciudad;
                amigo.Email = a.Email;


                if (a.Foto != null)
                {
                    // Si el usuario sube una foto debe borrarse la anterior
                    //Compruebo si tenia foto anterior
                    if (a.rutaFotoExistente != null)
                    {
                        //La ruta de la foto que tenia  
                        string ruta = Path.Combine(hosting.WebRootPath, "images", a.rutaFotoExistente);

                        //La borro
                        System.IO.File.Delete(ruta);
                    }
                                      
                    //Guardo la nueva foto en la ruta de images
                    amigo.rutaFoto = SubirImagen(a);
                }

                Amigo amigoModificado = amigoAlmacen.modificar(amigo);

                return RedirectToAction("index");
            }

            //vuelvo a la misma vista si no es valido para mostrar los errores de validacion
            return View(a);
        }

        private string SubirImagen (EditarAmigoModelo model)
        {
            string nombreFichero = null;

            if (model.Foto != null)
            {
                string carpetaSubida = Path.Combine(hosting.WebRootPath, "images");
                
                nombreFichero = Guid.NewGuid().ToString() + "_" + model.Foto.FileName;

                string ruta = Path.Combine(carpetaSubida, nombreFichero);

                //Otra forma de subir foto al servidor
                using (var fileStream = new FileStream(ruta, FileMode.Create))
                {
                    model.Foto.CopyTo(fileStream);
                }

            }

            return nombreFichero;
        }
    }
}
