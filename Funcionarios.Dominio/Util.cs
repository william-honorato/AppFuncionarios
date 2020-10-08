using System;
using System.Collections.Generic;
using System.Text;

namespace Funcionarios.Dominio
{
    public static class Util
    {
        public static void ValidarTamanhoString(string valor, string nomeCampo, uint tamanhoMinimo, uint tamanhoMaximo)
        {
            if(valor.Length >= tamanhoMinimo &&
               valor.Length <= tamanhoMaximo &&
               !string.IsNullOrEmpty(valor))
            {
                throw new ExcecaoDominio($"O campo {nomeCampo} deve ter entre {tamanhoMinimo} e {tamanhoMaximo} caracteres.");
            }
        }

        public static bool ValidarEmail(string valor)
        {
            return (!string.IsNullOrEmpty(valor));
        }

        public static DateTime ValidarData(string valor, string nomeCampo)
        {
            DateTime data;
            if (!DateTime.TryParse(valor, out data)) throw new ExcecaoDominio($"{nomeCampo} inválida");
            if(data.CompareTo(DateTime.Now) < 0) throw new ExcecaoDominio($"{nomeCampo} tem que ser menor que a data atual");

            return data;
        }

        public static string LimparString(string valor)
        {
            valor = valor ?? "";
            return valor.Trim();
        }
    }
}
