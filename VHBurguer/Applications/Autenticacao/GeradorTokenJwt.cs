using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VHBurguer.Domains;
using VHBurguer.Exceptions;

namespace VHBurguer.Applications.Autenticacao
{
    public class GeradorTokenJwt
    {
        private readonly IConfiguration _config;

        public GeradorTokenJwt (IConfiguration config)
        {
                       _config = config;
        }

        public string GerarToken(Usuario usuario)
        {
            var chave = _config["Jwt:Key"]!;

            var issuer = _config["Jwt:Issuer"]!;

            var audience = _config["Jwt:Audience"]!;

            var expiraEmMinutos = int.Parse(_config["Jwt:ExpireEmMinutos)"]!);

            var keyBytes = Encoding.UTF8.GetBytes(chave);

            if(keyBytes.Length < 32)
            {
                throw new DomainException("Jet: Key precisa ter precisa ter pelo menos 32 caracteres (256 bits).");
            }

            var securityKey = new SymmetricSecurityKey(keyBytes);

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioID.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.Nome)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(expiraEmMinutos),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
