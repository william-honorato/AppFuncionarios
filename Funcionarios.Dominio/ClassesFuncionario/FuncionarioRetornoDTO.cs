using System;
using System.Collections.Generic;
using System.Text;

namespace Funcionarios.Dominio.ClassesFuncionario
{
    public class FuncionarioRetornoDTO
    {
        public int ID { get; set; }

        public string Usuario { get; set; }

        public string Nome { get; set; }

        public DateTime? DataNascimento { get; set; }

        public string Email { get; set; }

        public static FuncionarioRetornoDTO Criar(Funcionario f)
        {
            var funcionarioDTO = new FuncionarioRetornoDTO()
            {
                ID = f.ID,
                Usuario = f.Usuario,
                Nome = f.Nome,
                DataNascimento = f.DataNascimento,
                Email = f.Email
            };

            return funcionarioDTO;
        }
    }
}
