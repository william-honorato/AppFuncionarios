using System;
using System.Collections.Generic;
using System.Text;

namespace Funcionarios.Dominio
{
    public class ExcecaoDominio : Exception
    {
        public ExcecaoDominio(string mensagemErro) : base(mensagemErro) {}

        public static void Validar(bool ehValido, string mensagemErro)
        {
            if (!ehValido) throw new ExcecaoDominio(mensagemErro);
        }
    }
}
