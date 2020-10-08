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

namespace Funcionarios.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuncionariosController : ControllerBase
    {
        private readonly AplicacaoDbContext _context;

        public FuncionariosController(AplicacaoDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult<dynamic> Autenticar([FromBody] Login user, [FromServices]IConfiguration configuration)
        {
            var userLogin = _context.Logins
                                    .AsNoTracking()
                                    .Where(l => l.Usuario == user.Usuario &&
                                                l.Senha   == user.Senha)
                                    .FirstOrDefault();

            if (userLogin == null)
                return NotFound(new { Message = "Usuário ou/e senha inválida(s)" });

            var token = ServicoToken.GerarToken(userLogin, configuration);

            return new { usuario = userLogin, token = token, DataHora = DateTime.Now };
        }

        // GET: api/Funcionarios
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Funcionario>>> GetFuncionarios()
        {
            return await _context.Funcionarios.ToListAsync();
        }

        // GET: api/Funcionarios/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Funcionario>> GetFuncionario(int id)
        {
            var funcionario = await _context.Funcionarios.FindAsync(id);

            if (funcionario == null) return NotFound();

            return funcionario;
        }

        // PUT: api/Funcionarios/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutFuncionario(int id, Funcionario funcionario)
        {
            if (id != funcionario.FuncionarioID) return BadRequest();

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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        //[Authorize]
        public async Task<ActionResult<Funcionario>> PostFuncionario(Funcionario funcionario)
        {
            _context.Funcionarios.Add(funcionario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFuncionario", new { id = funcionario.FuncionarioID }, funcionario);
        }

        // DELETE: api/Funcionarios/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Funcionario>> DeleteFuncionario(int id)
        {
            var funcionario = await _context.Funcionarios.FindAsync(id);
            if (funcionario == null)
            {
                return NotFound();
            }

            _context.Funcionarios.Remove(funcionario);
            await _context.SaveChangesAsync();

            return funcionario;
        }

        private bool FuncionarioExists(int id)
        {
            return _context.Funcionarios.Any(e => e.FuncionarioID == id);
        }
    }
}
