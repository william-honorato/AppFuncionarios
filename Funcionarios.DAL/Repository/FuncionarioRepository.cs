using Funcionarios.Dominio.Entidades.ClassesFuncionario;
using Funcionarios.Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcionarios.DAL.Repository
{
    public class FuncionarioRepository : Repository<Funcionario>, IFuncionarioRepository
    {
        public FuncionarioRepository(AplicacaoDbContext ctx) : base(ctx) {}

        public Task<Funcionario> ValidarAcessoUsuario(FuncionarioLoginDTO user)
        {
            return DbSet.AsNoTracking()
                        .Where(l => l.Usuario == user.Usuario &&
                                        l.Senha == user.Senha)
                        .FirstOrDefaultAsync();
        }
    }
}
