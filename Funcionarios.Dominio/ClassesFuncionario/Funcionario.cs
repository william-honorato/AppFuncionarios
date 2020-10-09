using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Funcionarios.Dominio
{
    public class Funcionario
    {
        //Para o EF Core
        private Funcionario() { }

        public Funcionario(string usuario, string senha)
        {
            SetarUsuario(usuario);
            SetarSenha(senha);
        }
        
        public Funcionario(string usuario, string senha, string nome, string dataNascimento, string email)
        {
            SetarUsuario(usuario);
            SetarSenha(senha);
            SetarNome(nome);
            SetarDataNascimento(dataNascimento);
            SetarEmail(email);
        }

        public int ID { get; set; }

        public string Nome { get; set; }

        public DateTime? DataNascimento { get; set; }

        public string Email { get; set; }

        public string Usuario { get; set; }

        public string Senha { get; set; }

        public void SetarUsuario(string usuario)
        {
            usuario = Util.LimparString(usuario);
            Util.ValidarTamanhoString(usuario, "Usuário", Util.TAMANHO_MINIMO_USUARIO, Util.TAMANHO_MAXIMO_USUARIO);
            Usuario = usuario;
        }

        public void SetarSenha(string senha)
        {
            senha = Util.LimparString(senha);
            Util.ValidarTamanhoString(senha, "Senha", Util.TAMANHO_MINIMO_SENHA, Util.TAMANHO_MAXIMO_SENHA);
            Senha = senha;
        }

        public void SetarNome(string nome)
        {
            nome = Util.LimparString(nome);
            Util.ValidarTamanhoString(nome, "Nome", Util.TAMANHO_MINIMO_NOME, Util.TAMANHO_MAXIMO_NOME);
            ValidarSobrenome(nome);
            Nome = nome;
        }

        public void SetarDataNascimento(string data)
        {
            data = Util.LimparString(data);
            DataNascimento = Util.ValidarData(data, "Data de Nascimento");
        }

        public void SetarEmail(string email)
        {
            email = Util.LimparString(email);
            Util.ValidarTamanhoString(email, "Email", Util.TAMANHO_MINIMO_EMAIL, Util.TAMANHO_MAXIMO_EMAIL);
            Email = email;
        }

        private bool ValidarSobrenome(string nome)
        {
            return nome.Contains(" ");
        }
    }
}