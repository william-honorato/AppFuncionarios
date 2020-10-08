using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Funcionarios.Dominio
{
    public class Login
    {
        private static readonly uint TAMANHO_MINIMO_USUARIO = 5;
        private static readonly uint TAMANHO_MAXIMO_USUARIO = 50;

        private static readonly uint TAMANHO_MINIMO_SENHA = 8;
        private static readonly uint TAMANHO_MAXIMO_SENHA = 50;

        //Para o EF Core
        private Login() { }

        public Login(string usuario, string senha)
        {
            SetUsuario(usuario);
            SetSenha(senha);
        }

        public int LoginID { get; set; }

        [MaxLength(50)]
        [Required]
        public string Usuario { get; private set; }

        [MaxLength(50)]
        [Required]
        public string Senha { get; private set; }

        public int? FuncionarioID { get; set; }

        public Funcionario Funcionario { get; set; }

        public void SetUsuario(string usuario)
        {
            usuario = Util.LimparString(usuario);
            Util.ValidarTamanhoString(usuario, "Usuário", TAMANHO_MINIMO_USUARIO, TAMANHO_MAXIMO_USUARIO);
            Usuario = usuario;  
        }

        public void SetSenha(string senha)
        {
            senha = Util.LimparString(senha);
            Util.ValidarTamanhoString(senha, "Senha", TAMANHO_MINIMO_SENHA, TAMANHO_MAXIMO_SENHA);
            Senha = senha;
        }
    }
}
