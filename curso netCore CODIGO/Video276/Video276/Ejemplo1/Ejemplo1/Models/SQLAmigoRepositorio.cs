using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo1.Models
{
    public class SQLAmigoRepositorio:IAmigoAlmacen
    {
        private readonly AppDbContext contexto;
        private List<Amigo> amigosLista;

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

        public Amigo borrar(int Id)
        {
            Amigo amigo = contexto.Amigos.Find(Id);
            if (amigo != null)
            {
                contexto.Amigos.Remove(amigo);
                contexto.SaveChanges();
            }
            return amigo;
        }

        public List<Amigo> DameTodosLosAMigos()
        {
            amigosLista = contexto.Amigos.ToList<Amigo>();

            return amigosLista;
        }

        public Amigo dameDatosAmigo(int Id)
        {
            return contexto.Amigos.Find(Id);
        }

        public Amigo modificar(Amigo amigo)
        {
            var employee = contexto.Amigos.Attach(amigo);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            contexto.SaveChanges();
            return amigo;
        }
    }
}
