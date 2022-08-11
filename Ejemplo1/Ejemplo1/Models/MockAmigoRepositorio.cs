using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo1.Models
{
    public class MockAmigoRepositorio: IAmigoAlmacencs
    {
        private List<Amigo> amigoLista;

        public MockAmigoRepositorio()
        {
            amigoLista = new List<Amigo>();
            amigoLista.Add(new Amigo() { Id = 1, Nombre = "Pedro", Ciudad = Provincia.Jujuy, Email = "Pedro@mail.com" });
            amigoLista.Add(new Amigo() { Id = 2, Nombre = "Sapo", Ciudad = Provincia.Salta, Email = "Sapo@mail.com" });
            amigoLista.Add(new Amigo() { Id = 3, Nombre = "Roto", Ciudad = Provincia.Tucumán, Email = "Roto@mail.com" });

        }

        
        public Amigo dameDatosAmigo(int id)
        {
            return this.amigoLista.FirstOrDefault(e => e.Id == id);
        }

        //El metodo que obtiene toda la lista que pienso mostrarla con ListView
        public List<Amigo> DameTodosLosAmigos()
        {
            return this.amigoLista;
        }

        //CREATE
        public Amigo nuevo(Amigo amigo)
        {
            //Busco el mayor numero de ID le sumo uno y se lo asigno al nuevo objeto
            amigo.Id = amigoLista.Max(a => a.Id) + 1;

            amigoLista.Add(amigo);

            return amigo;
        }

        public Amigo modificar(Amigo modificarAmigo)
        {
            Amigo amigo = amigoLista.FirstOrDefault(e => e.Id == modificarAmigo.Id);
            
            if (amigo != null)
            {
                amigo.Nombre = modificarAmigo.Nombre;
                amigo.Email = modificarAmigo.Email;
                amigo.Ciudad = modificarAmigo.Ciudad;
            }

            return amigo;

        }

        public Amigo borrar(int id)
        {
            Amigo amigo = amigoLista.FirstOrDefault(e => e.Id == id);
            if (amigo != null)
            {
                amigoLista.Remove(amigo);
            }

            return amigo;
        }

    }
}
