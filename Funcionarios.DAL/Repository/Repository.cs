using Funcionarios.Dominio;
using Funcionarios.Dominio.Entidades;
using Funcionarios.Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcionarios.DAL.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : Entity, new()
    {
        protected readonly AplicacaoDbContext Db;
        protected readonly DbSet<T> DbSet;

        protected Repository(AplicacaoDbContext db)
        {
            Db = db;
            DbSet = db.Set<T>();
        }

        public async Task<int> Adicionar(T obj)
        {
            DbSet.Add(obj);
            return await SalvarAlteracoes();
        }

        public async Task<int> Atualizar(T obj)
        {
            Db.Entry(obj).State = EntityState.Modified;
            return await Db.SaveChangesAsync();
        }

        public async Task<int> Remover(int idObjeto)
        {
            DbSet.Remove(new T { ID = idObjeto });
            return await SalvarAlteracoes();
        }

        public async Task<T> BuscarPorId(int idObjeto)
        {
            return await DbSet.FindAsync(idObjeto);
        }

        public async Task<IList<T>> TrazerTodos(int qtdMaximaRetorno = 1000)
        {
            return await DbSet.Take(qtdMaximaRetorno).ToListAsync();
        }

        public async Task<int> SalvarAlteracoes()
        {
            return await Db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Db?.Dispose();
        }
    }
}
