using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ejemplo1.Utilidades
{
    public class ValidarNombreUsuario: ValidationAttribute
    {
        private readonly string usuario;

        public ValidarNombreUsuario(string usuario)
        {
            this.usuario = usuario;
        }


        //La clase validationAttribute es una clase abstracta por lo que hay que redifinir con override los metodos que quiera usar
        public override bool IsValid(object value)
        {
            Boolean permitido = true;

            if (value.ToString().Contains("puto"))
                permitido = false;

            return permitido;
        }
    }
}
