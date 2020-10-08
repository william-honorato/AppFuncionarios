using Funcionarios.Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Funcionarios.DAL
{
    public class AplicacaoDbContext : DbContext
    {
        public AplicacaoDbContext(DbContextOptions<AplicacaoDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Login>()
                        .HasOne(b => b.Funcionario)
                        .WithOne(i => i.Login)
                        .HasForeignKey<Login>(b => b.FuncionarioID)
                        .IsRequired(false);
                            
            //modelBuilder.ApplyConfiguration(new LoginMap());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Login> Logins { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
    }

    public class LoginMap : IEntityTypeConfiguration<Login>
    {
        public void Configure(EntityTypeBuilder<Login> builder)
        {
            builder.HasKey(l => l.LoginID);

            builder.OwnsOne(l => l.Funcionario, fun =>
            {
                fun.Property(f => f.Nome);
                fun.Property(f => f.DataNascimento);
                fun.Property(l => l.Email);
            });
        }
    }
}