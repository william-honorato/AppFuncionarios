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
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace Funcionarios.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuncionariosController : ControllerBase
    {
        private readonly int RETORNO_MAXIMO_FUNCIONARIOS = 100;
        private readonly AplicacaoDbContext _context;
        private static string MSG_ERRO_SERVIDOR = "Erro interno no servidor: ";

        public FuncionariosController(AplicacaoDbContext context)
        {
            _context = context;
        }

        // POST: api/Funcionarios/login
        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult<dynamic> Autenticar([FromBody] FuncionarioLoginDTO user, [FromServices]IConfiguration configuration)
        {
            Funcionario funcionarioLogin = null;
            string token = "";
            try
            {
                funcionarioLogin = ValidarAcessoUsuario(user);

                if (funcionarioLogin == null)
                    return StatusCode((int)HttpStatusCode.Unauthorized, "Usuário ou/e senha inválida(s)");

                token = ServicoToken.GerarToken(funcionarioLogin, configuration);
            }
            catch (Exception ex)
            {
                var msgErro = $"{MSG_ERRO_SERVIDOR}{ex.Message} {ex?.InnerException?.Message}";
                return StatusCode((int)HttpStatusCode.InternalServerError, msgErro);
            }

            return new { funcionarioLogin = FuncionarioRetornoDTO.Criar(funcionarioLogin), token = token, dataHora = DateTime.Now };
        }

        // GET: api/Funcionarios
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<FuncionarioRetornoDTO>>> TrazerFuncionarios()
        {
            List<FuncionarioRetornoDTO> funcionarios;

            try
            {
                funcionarios = await (from f in _context.Funcionarios
                                      select new FuncionarioRetornoDTO()
                                      {
                                            ID = f.ID,
                                            Usuario = f.Usuario,
                                            Nome = f.Nome,
                                            DataNascimento = f.DataNascimento,
                                            Email = f.Email
                                      })
                                      .Take(RETORNO_MAXIMO_FUNCIONARIOS)
                                      .ToListAsync();
            }
            catch (Exception ex)
            {
                var msgErro = $"{MSG_ERRO_SERVIDOR}{ex.Message} {ex?.InnerException?.Message}";
                return StatusCode((int)HttpStatusCode.InternalServerError, msgErro);
            }
            
            return funcionarios;
        }

        // GET: api/Funcionarios/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Funcionario>> TrazerFuncionario(int id)
        {
            Funcionario funcionario;

            try
            {
                funcionario = await _context.Funcionarios.FindAsync(id);

                if (funcionario == null) return NotFound();
            }
            catch (Exception ex)
            {
                var msgErro = $"{MSG_ERRO_SERVIDOR}{ex.Message} {ex?.InnerException?.Message}";
                return StatusCode((int)HttpStatusCode.InternalServerError, msgErro);
            }

            return funcionario;
        }

        // PUT: api/Funcionarios/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> AtualizarFuncionario(int id, FuncionarioDTO funcionarioDTO)
        {
            try
            {
                if (id != funcionarioDTO.ID) return BadRequest();

                _context.Entry(funcionarioDTO.CriarFuncionario()).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var msgErro = $"{MSG_ERRO_SERVIDOR}{ex.Message} {ex?.InnerException?.Message}";
                return StatusCode((int)HttpStatusCode.InternalServerError, msgErro);
            }

            return StatusCode((int)HttpStatusCode.Accepted, "Atualizado com sucesso");
        }

        // POST: api/Funcionarios/novo-usuario
        [HttpPost("novo-usuario")]
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
                var msgErro = $"{MSG_ERRO_SERVIDOR}{ex.Message} {ex?.InnerException?.Message}";
                return StatusCode((int)HttpStatusCode.InternalServerError, msgErro);
            }

            return CreatedAtAction("TrazerFuncionario", new { id = funcionario.ID }, funcionario);
        }

        // POST: api/Funcionarios
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Funcionario>> CriarFuncionario(FuncionarioDTO funcionarioDTO)
        {
            Funcionario funcionario;
            try
            {
                funcionario = funcionarioDTO.CriarFuncionario();
                _context.Funcionarios.Add(funcionario);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var msgErro = $"{MSG_ERRO_SERVIDOR}{ex.Message} {ex?.InnerException?.Message}";
                return StatusCode((int)HttpStatusCode.InternalServerError, msgErro);
            }

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
                var msgErro = $"{MSG_ERRO_SERVIDOR}{ex.Message} {ex?.InnerException?.Message}";
                return StatusCode((int)HttpStatusCode.InternalServerError, msgErro);
            }

            return funcionario;
        }

        private bool FuncionarioExiste(int id)
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
            funcionarios.ForEach(f => f.SetarSenha(""));
        }
    }
}
