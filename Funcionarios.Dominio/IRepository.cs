using System;
using System.Collections.Generic;
using System.Text;

namespace Funcionarios.Dominio
{
    public interface IRepository<TipoObjeto> where TipoObjeto : class
    {
        TipoObjeto Salvar(TipoObjeto obj);
        TipoObjeto TrazerObjeto(int idObjeto);
        TipoObjeto Atualizar(TipoObjeto obj);
        TipoObjeto Excluir(int idObjeto);
        IList<TipoObjeto> TrazerTodos();
    }
}
