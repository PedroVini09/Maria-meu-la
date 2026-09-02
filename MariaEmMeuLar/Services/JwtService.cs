using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MariaEmMeuLar.Models;
using Microsoft.IdentityModel.Tokens;

namespace MariaEmMeuLar.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GerarToken(UsuarioAdmin usuario)
        {
            var key = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var expirationMinutes = _configuration.GetValue<int>("Jwt:ExpirationMinutes");

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidOperationException("A chave JWT não foi configurada");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,usuario.Id.ToString()),//identificar 

                new Claim(ClaimTypes.Role,"Admin")//Pertence
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer:issuer, audience:audience,claims:claims,expires:DateTime.UtcNow.AddMinutes(expirationMinutes), signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
            .WriteToken(token);
        }
    }
}