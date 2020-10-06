using System;
using System.Collections.Generic;
using System.Text;

namespace Funcionarios.Dominio
{
    public static class Util
    {
        public static bool ValidarTamanhoString(string valor, uint tamanhoMinimo, uint tamanhoMaximo)
        {
            return (valor.Length >= tamanhoMinimo && 
                    valor.Length <= tamanhoMaximo &&
                    !string.IsNullOrEmpty(valor));
        }

        public static bool ValidarEmail(string valor)
        {
            return (!string.IsNullOrEmpty(valor));
        }

        public static bool ValidarData(string valor)
        {
            return (!string.IsNullOrEmpty(valor));
        }
    }
}
