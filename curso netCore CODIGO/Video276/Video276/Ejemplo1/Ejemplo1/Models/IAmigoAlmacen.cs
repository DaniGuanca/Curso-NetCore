using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo1.Models
{
    public interface IAmigoAlmacen
    {
        Amigo dameDatosAmigo(int Id);
        List<Amigo> DameTodosLosAMigos();
        Amigo nuevo(Amigo amigo);

        //Agregamos un par de métodos más para hacer un CRUD
        Amigo modificar(Amigo modificarAmigo);
        Amigo borrar(int id);

     
    }
}
