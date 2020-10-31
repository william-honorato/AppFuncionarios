namespace Funcionarios.API.Servicos
{
    public static class ServicoHash
    {
        //Devolve a hash com 60 caracteres, independente do tamanho do valor passado
        //Testado com valores 1, 5, 10 e 100 caracteres
        public static string Gerar(string valor)
        {
            var valorHash = BCrypt.Net.BCrypt.HashPassword(valor);
            return valorHash;
        }

        public static bool Verificar(string valor, string valorHash)
        {
            return BCrypt.Net.BCrypt.Verify(valor, valorHash);
        }
    }
}