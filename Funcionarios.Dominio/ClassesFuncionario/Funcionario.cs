using System;
using System.ComponentModel.DataAnnotations;

namespace Funcionarios.Dominio
{
    public class Funcionario
    {
        public int FuncionarioID { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public int LoginID { get; set; }

        public Login Login { get; set; }
    }
}