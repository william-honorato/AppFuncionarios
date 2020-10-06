using System;
using System.Collections.Generic;
using System.Text;

namespace Funcionarios.Dominio
{
    public interface IRepository<TipoObjeto> where TipoObjeto : class
    {
        TipoObjeto Adicionar(TipoObjeto obj);
        TipoObjeto TrazerObjeto(int idObjeto);
        TipoObjeto Atualizar(TipoObjeto obj);
        TipoObjeto Remover(int idObjeto);
        IList<TipoObjeto> TrazerTodos();
    }
}
