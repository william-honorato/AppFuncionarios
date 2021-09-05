using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Funcionarios.DAL;
using Microsoft.AspNetCore.Authorization;
using Funcionarios.API.Servicos;
using Microsoft.Extensions.Configuration;
using Funcionarios.Dominio.Entidades.ClassesFuncionario;
using System.Net;
using Funcionarios.Dominio.Interfaces;

namespace Funcionarios.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuncionariosController : ControllerBase
    {
        private readonly int RETORNO_MAXIMO_FUNCIONARIOS = 100;
        private readonly IFuncionarioRepository _context;
        private static string MSG_ERRO_SERVIDOR = "Erro interno no servidor: ";

        public FuncionariosController(IFuncionarioRepository context)
        {
            _context = context;
        }

        // POST: api/Funcionarios/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Autenticar([FromBody] FuncionarioLoginDTO user, [FromServices]IConfiguration configuration)
        {
            Funcionario funcionarioLogin = null;
            string token = "";
            try
            {
                funcionarioLogin = await _context.ValidarAcessoUsuario(user);

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
        //[Authorize]
        //TODO: Remover comentário
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<FuncionarioRetornoDTO>>> TrazerFuncionarios()
        {
            List<FuncionarioRetornoDTO> funcionariosRetorno;

            try
            {
                var funcionarios = await _context.TrazerTodos(RETORNO_MAXIMO_FUNCIONARIOS);

                funcionariosRetorno = (from f in funcionarios
                                      select new FuncionarioRetornoDTO()
                                      {
                                            ID = f.ID,
                                            Usuario = f.Usuario,
                                            Nome = f.Nome,
                                            DataNascimento = f.DataNascimento,
                                            Email = f.Email
                                      }).ToList();
            }
            catch (Exception ex)
            {
                var msgErro = $"{MSG_ERRO_SERVIDOR}{ex.Message} {ex?.InnerException?.Message}";
                return StatusCode((int)HttpStatusCode.InternalServerError, msgErro);
            }
            
            return funcionariosRetorno;
        }

        // GET: api/Funcionarios/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Funcionario>> TrazerFuncionario(int id)
        {
            Funcionario funcionario;

            try
            {
                funcionario = await _context.BuscarPorId(id);

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

                await _context.Atualizar(funcionarioDTO.CriarFuncionario());
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
                await _context.Adicionar(funcionario);
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
                await _context.Adicionar(funcionario);
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
                funcionario = await _context.BuscarPorId(id);
                if (funcionario == null)
                {
                    return NotFound();
                }

                await _context.Remover(id);
            }
            catch (Exception ex)
            {
                var msgErro = $"{MSG_ERRO_SERVIDOR}{ex.Message} {ex?.InnerException?.Message}";
                return StatusCode((int)HttpStatusCode.InternalServerError, msgErro);
            }

            return funcionario;
        }
    }
}
