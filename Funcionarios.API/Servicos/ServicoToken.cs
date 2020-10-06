using Funcionarios.Dominio;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Funcionarios.API.Servicos
{
    public class ServicoToken
    {
        private readonly IConfiguration config;

        public ServicoToken(IConfiguration configuration) 
        {
            config = configuration;
        }

        public string GerarToken(Funcionario funcionario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, funcionario.Login.Usuario)
                });
            var chave = config.GetValue<string>("ChaveToken");
            var key = Encoding.ASCII.GetBytes(chave);
            var securityKey = new SymmetricSecurityKey(key);
            var expires = DateTime.UtcNow.AddMinutes(5);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var tokenDesriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                Expires = expires,
                SigningCredentials = signingCredentials
            };

            var token = tokenHandler.CreateToken(tokenDesriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
