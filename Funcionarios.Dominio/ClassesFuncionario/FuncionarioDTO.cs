using System;
using System.Collections.Generic;
using System.Text;

namespace Funcionarios.Dominio.ClassesFuncionario
{
    public class FuncionarioDTO
    {
        public string Nome { get; set; }

        public DateTime DataNascimento { get; set; }

        public string Email { get; set; }

        public string Usuario { get; set; }

        public string Senha { get; set; }

        public Funcionario CriarFuncionario()
        {
            var funcionario = new Funcionario(Nome, Senha, Email);
            return funcionario;
        }
    }
}
