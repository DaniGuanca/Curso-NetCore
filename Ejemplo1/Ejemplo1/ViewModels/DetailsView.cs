using Ejemplo1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo1.ViewModels
{
    public class DetailsView
    {
        //Este es mi objeto personalizado, comienzo a poner las propiedades que voy a mostrar en la vista. En mi caso yo queria mostrar
        //mostrar el objeto Amigo pero tambien un titulo a modo de ejemplo, como la clase Amigo no tiene propiedad titulo, hago esta
        //clase personalizada ViewModel con la propiedad titulo que necesitaria. Lo mismo con subtitulo

        //Un titulo y subtitulo para la vista
        public string Titulo { get; set; }
        public string Subtitulo { get; set; }

        //El objeto amigo
        public Amigo amigo { get; set; }
    }
}
