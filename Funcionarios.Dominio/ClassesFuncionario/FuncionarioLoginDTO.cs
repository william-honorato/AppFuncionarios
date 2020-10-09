using System;
using System.Collections.Generic;
using System.Text;

namespace Funcionarios.Dominio.ClassesFuncionario
{
    public class FuncionarioLoginDTO
    {
        public string Usuario { get; set; }

        public string Senha { get; set; }

        public Funcionario CriarFuncionario()
        {
            var funcionario = new Funcionario(Usuario, Senha);
            return funcionario;
        }
    }
}
