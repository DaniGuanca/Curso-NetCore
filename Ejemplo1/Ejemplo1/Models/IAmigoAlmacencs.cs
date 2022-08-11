using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo1.Models
{
    public interface IAmigoAlmacencs
    {
        //Acordarse que las interfaces no tienen definicion de metodos. Esta en las explicaciones del curso POO con PHP de Jon
        Amigo dameDatosAmigo(int id);

        //El metodo que va a devolver una lista de objetos para mostrar en listview
        List<Amigo> DameTodosLosAmigos();

        Amigo nuevo(Amigo amigo);

        Amigo modificar(Amigo modificarAmigo);

        Amigo borrar(int id);
    }
}
