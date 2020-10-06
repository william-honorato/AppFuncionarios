using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Funcionarios.Dominio
{
    public class Login
    {
        public int LoginID { get; private set; }

        [Required]
        public string Usuario { get; private set; }

        [Required]
        public string Senha { get; private set; }

        public Funcionario Funcionario { get; private set; }
    }
}
