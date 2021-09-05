using Funcionarios.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Funcionarios.Dominio.Interfaces
{
    public interface IRepository<TipoObjeto> : IDisposable where TipoObjeto : Entity
    {
        Task<int> Adicionar(TipoObjeto obj);
        Task<TipoObjeto> BuscarPorId(int idObjeto);
        Task<int> Atualizar(TipoObjeto obj);
        Task<int> Remover(int idObjeto);
        Task<IList<TipoObjeto>> TrazerTodos(int qtdMaximaRetorno);
    }
}
