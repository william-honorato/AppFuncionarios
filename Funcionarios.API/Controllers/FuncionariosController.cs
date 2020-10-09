using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Funcionarios.DAL;
using Funcionarios.Dominio;
using Microsoft.AspNetCore.Authorization;
using Funcionarios.API.Servicos;
using Microsoft.Extensions.Configuration;
using Funcionarios.Dominio.ClassesFuncionario;

namespace Funcionarios.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuncionariosController : ControllerBase
    {
        private readonly int RETORNO_MAXIMO_FUNCIONARIOS = 100;
        private readonly AplicacaoDbContext _context;

        public FuncionariosController(AplicacaoDbContext context)
        {
            _context = context;
        }

        // POST: api/Funcionarios/login
        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult<dynamic> Autenticar([FromBody] FuncionarioLoginDTO user, [FromServices]IConfiguration configuration)
        {
            Funcionario userLogin = null;
            string token = "";
            try
            {
                userLogin = ValidarAcessoUsuario(user);

                if (userLogin == null)
                    return NotFound(new { Menssagem = "Usuário ou/e senha inválida(s)" });

                token = ServicoToken.GerarToken(userLogin, configuration);
            }
            catch (Exception ex)
            {
                return NotFound(new { Menssagem = ex.Message + " " + ex?.InnerException?.Message });
            }

            userLogin.Senha = "";
            return new { usuario = userLogin, token = token, DataHora = DateTime.Now };
        }

        // GET: api/Funcionarios
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Funcionario>>> TrazerFuncionarios()
        {
            var funcionarios = await _context.Funcionarios.Take(RETORNO_MAXIMO_FUNCIONARIOS).ToListAsync();
            RemoverSenha(funcionarios);
            return funcionarios;
        }

        // GET: api/Funcionarios/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Funcionario>> TrazerFuncionario(int id)
        {
            var funcionario = await _context.Funcionarios.FindAsync(id);

            if (funcionario == null) return NotFound();

            funcionario.Senha = "";
            return funcionario;
        }

        // PUT: api/Funcionarios/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> AtualizarFuncionario(int id, Funcionario funcionario)
        {
            if (id != funcionario.ID) return BadRequest();

            _context.Entry(funcionario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FuncionarioExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // POST: api/Funcionarios
        [HttpPost("criar")]
        public async Task<ActionResult<dynamic>> CriarFuncionario(FuncionarioLoginDTO funcionarioLogin)
        {
            Funcionario funcionario = null;

            try
            {
                funcionario = funcionarioLogin.CriarFuncionario();
                _context.Funcionarios.Add(funcionario);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound(new { Menssagem = ex.Message + " " + ex?.InnerException?.Message });
            }

            funcionario.Senha = "";
            return CreatedAtAction("TrazerFuncionario", new { id = funcionario.ID }, funcionario);
        }

        // POST: api/Funcionarios
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Funcionario>> CriarFuncionario(Funcionario funcionario)
        {
            try
            {
                _context.Funcionarios.Add(funcionario);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound(new { Menssagem = ex.Message + " " + ex?.InnerException?.Message });
            }

            funcionario.Senha = "";
            return CreatedAtAction("TrazerFuncionario", new { id = funcionario.ID }, funcionario);
        }

        // DELETE: api/Funcionarios/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Funcionario>> DeletarFuncionario(int id)
        {
            Funcionario funcionario;

            try
            {
                funcionario = await _context.Funcionarios.FindAsync(id);
                if (funcionario == null)
                {
                    return NotFound();
                }

                _context.Funcionarios.Remove(funcionario);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound(new { Menssagem = ex.Message + " " + ex?.InnerException?.Message });
            }

            funcionario.Senha = "";
            return funcionario;
        }

        private bool FuncionarioExists(int id)
        {
            return _context.Funcionarios.Any(e => e.ID == id);
        }

        private Funcionario ValidarAcessoUsuario(FuncionarioLoginDTO user)
        {
            return _context.Funcionarios
                           .AsNoTracking()
                           .Where(l => l.Usuario == user.Usuario &&
                                         l.Senha == user.Senha)
                           .FirstOrDefault();
        }

        private void RemoverSenha(List<Funcionario> funcionarios)
        {
            funcionarios.ForEach(f => f.Senha = "");
        }
    }
}
