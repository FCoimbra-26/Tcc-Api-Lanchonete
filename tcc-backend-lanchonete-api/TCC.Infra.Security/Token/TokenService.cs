using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TCC.Domain.Entities;
using TCC.Infra.Security.Interfaces;
using TCC.Infra.Security.Secrets;

namespace TCC.Infra.Security.Token
{
    public class TokenService : ITokenService
    {
        public string GenerateToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.secret);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim("Id", usuario.Id.ToString())
            };

            if (usuario.Roles != null && usuario.Roles.Any())
            {
                foreach (var role in usuario.Roles.Where(r => r.Ativo))
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Role.ToString()));
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
