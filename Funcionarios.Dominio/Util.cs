using System;

namespace Funcionarios.Dominio
{
    public static class Util
    {
        public static readonly int TAMANHO_MINIMO_NOME = 5;
        public static readonly int TAMANHO_MAXIMO_NOME = 150;

        public static readonly int TAMANHO_MINIMO_USUARIO = 5;
        public static readonly int TAMANHO_MAXIMO_USUARIO = 50;

        public static readonly int TAMANHO_MINIMO_SENHA = 8;
        public static readonly int TAMANHO_MAXIMO_SENHA = 50;

        public static readonly int TAMANHO_MINIMO_EMAIL = 5;
        public static readonly int TAMANHO_MAXIMO_EMAIL = 100;

        public static void ValidarTamanhoString(string valor, string nomeCampo, int tamanhoMinimo, int tamanhoMaximo)
        {
            int tamanho = valor.Length;
            if ((tamanho < tamanhoMinimo) ||
                (tamanho > tamanhoMaximo) ||
                string.IsNullOrEmpty(valor))
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
