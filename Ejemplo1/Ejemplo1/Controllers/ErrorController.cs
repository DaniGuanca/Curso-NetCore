using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo1.Controllers
{
    public class ErrorController : Controller
    {
        //Para tener logs donde guardar errores
        private readonly ILogger<ErrorController> logs;
        
        public ErrorController(ILogger<ErrorController> log)
        {
            this.logs = log;
        }

        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "El recurso solicitado no existe";
                    break;
            }

            return View("Error");
        }

        //Para que no requiera validacion
        [AllowAnonymous]
        [Route ("Error")]
        public IActionResult Excepciones()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ViewBag.ExceptionPath = exceptionHandlerPathFeature.Path;
            ViewBag.ExceptionMessage = exceptionHandlerPathFeature.Error.Message;
            ViewBag.StackTrace = exceptionHandlerPathFeature.Error.StackTrace;

            //Guardo el error en el log
            logs.LogError($"Ruta del ERROR: {exceptionHandlerPathFeature.Path} +" +
                $"Excepcion: {exceptionHandlerPathFeature.Error.Message} +" +
                $"Traza del ERROR: {exceptionHandlerPathFeature.Error.StackTrace}");

            return View("ErrorGenerico");
        }

    }
}
