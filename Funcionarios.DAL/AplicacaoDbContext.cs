using Funcionarios.Dominio;
using Microsoft.EntityFrameworkCore;

namespace Funcionarios.DAL
{
    public class AplicacaoDbContext : DbContext
    {
        public AplicacaoDbContext(DbContextOptions<AplicacaoDbContext> options) : base(options) {}

        public DbSet<Funcionario> Funcionarios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region Nome
            builder.Entity<Funcionario>()
                   .Property(b => b.Nome)
                   .HasMaxLength(Util.TAMANHO_MAXIMO_NOME)
                   .IsRequired(false);
            #endregion

            #region Data Nascimento
            builder.Entity<Funcionario>()
                   .Property(b => b.DataNascimento)
                   .IsRequired(false);
            #endregion

            #region Email
            builder.Entity<Funcionario>()
                   .Property(b => b.Email)
                   .HasMaxLength(Util.TAMANHO_MAXIMO_EMAIL)
                   .IsRequired(false);

            builder.Entity<Funcionario>()
                   .HasIndex(f => f.Email)
                   .IsUnique();
            #endregion

            #region Usuario
            builder.Entity<Funcionario>()
                .Property(b => b.Usuario)
                .HasMaxLength(Util.TAMANHO_MAXIMO_USUARIO)
                .IsRequired();

            builder.Entity<Funcionario>()
                .HasIndex(f => f.Usuario)
                .IsUnique();
            #endregion

            #region Senha
            builder.Entity<Funcionario>()
                   .Property(b => b.Senha)
                   .HasMaxLength(Util.TAMANHO_MAXIMO_SENHA)
                   .IsRequired();
            #endregion
        }
    }
}