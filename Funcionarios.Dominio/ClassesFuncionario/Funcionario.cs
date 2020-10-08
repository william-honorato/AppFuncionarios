using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Funcionarios.Dominio
{
    public class Funcionario
    {
        private static readonly uint TAMANHO_MINIMO_NOME = 5;
        private static readonly uint TAMANHO_MAXIMO_NOME = 100;

        //Para o EF Core
        private Funcionario() { }

        public Funcionario(string nome, string dataNascimento, string email)
        {
            SetarNome(nome);
            SetarDataNascimento(dataNascimento);
            Email = email;
        }

        public int FuncionarioID { get; set; }

        [MaxLength(100)]
        [Required]
        public string Nome { get; private set; }

        [Required]
        public DateTime DataNascimento { get; private set; }

        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; private set; }

        public Login Login { get; set; }

        public void SetarNome(string nome)
        {
            nome = Util.LimparString(nome);
            Util.ValidarTamanhoString(nome, "Nome", TAMANHO_MINIMO_NOME, TAMANHO_MAXIMO_NOME);
            ValidarSobrenome(nome);
            Nome = nome;
        }

        public void SetarDataNascimento(string data)
        {
            data = Util.LimparString(data);
            DataNascimento = Util.ValidarData(data, "Data de Nascimento");
        }

        private bool ValidarSobrenome(string nome)
        {
            return nome.Contains(" ");
        }
    }
}