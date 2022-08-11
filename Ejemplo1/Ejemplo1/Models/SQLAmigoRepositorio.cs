using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo1.Models
{
    public class SQLAmigoRepositorio : IAmigoAlmacencs
    {

        private readonly AppDbContext contexto;

        private List<Amigo> amigosLista;


        //Constructor
        public SQLAmigoRepositorio(AppDbContext contexto)
        {
            this.contexto = contexto;
        }


        public Amigo nuevo(Amigo amigo)
        {
            contexto.Amigos.Add(amigo);

            contexto.SaveChanges();

            return amigo;
        }


        public Amigo modificar(Amigo modificarAmigo)
        {
            //Con attach me traigo todos las referencias y datos del objeto y los modifica por los nuevos valores
            var amigo = contexto.Amigos.Attach(modificarAmigo);

            amigo.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            contexto.SaveChanges();

            return modificarAmigo;
        }


        public Amigo borrar(int id)
        {
            Amigo amigo = contexto.Amigos.Find(id);

            if(amigo != null)
            {
                contexto.Amigos.Remove(amigo);
                contexto.SaveChanges();
            }

            return amigo;
        }


        public Amigo dameDatosAmigo(int id)
        {
            return contexto.Amigos.Find(id);
        }


        public List<Amigo> DameTodosLosAmigos()
        {
            amigosLista = contexto.Amigos.ToList<Amigo>();

            return amigosLista;
        }      

    }
}
