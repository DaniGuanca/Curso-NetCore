using Ejemplo1.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo1.Models
{
    public class AppDbContext:IdentityDbContext<UsuarioAplicacion>
    {
        public AppDbContext (DbContextOptions<AppDbContext> options): base(options)
        {

        }

        public DbSet<Amigo> Amigos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Amigo>().HasData(new Amigo
            //{
            //    Id = 1,
            //    Nombre = "Dani",
            //    Ciudad = Provincia.Jujuy,
            //    Email = "dany12rp@hotmail.com"
            //},
            //new Amigo
            //{
            //    Id = 2,
            //    Nombre = "Sapo",
            //    Ciudad = Provincia.Salta,
            //    Email = "asd@hotmail.com"
            //},
            //new Amigo
            //{
            //    Id = 3,
            //    Nombre = "Roto",
            //    Ciudad = Provincia.Tucumán,
            //    Email = "xxx@hotmail.com"
            //});
        }
    }
}
