using Funcionarios.Dominio.Entidades.ClassesFuncionario;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Funcionarios.Dominio.Interfaces
{
    public interface IFuncionarioRepository : IRepository<Funcionario> 
    {
        public Task<Funcionario> ValidarAcessoUsuario(FuncionarioLoginDTO user);
    }
}
