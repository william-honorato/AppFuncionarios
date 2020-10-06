using Funcionarios.Dominio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Funcionarios.DAL
{
    public class AplicacaoDbContext : DbContext
    {
        public AplicacaoDbContext(DbContextOptions<AplicacaoDbContext> options) : base(options) {}

        public DbSet<Login> Logins { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
    }
}